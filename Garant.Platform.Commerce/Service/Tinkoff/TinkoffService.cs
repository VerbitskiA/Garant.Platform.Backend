using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Commerce.Abstraction;
using Garant.Platform.Commerce.Abstraction.Tinkoff;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Commerce.Models.Tinkoff.Output;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Garant.Platform.Commerce.Service.Tinkoff
{
    /// <summary>
    /// Класс реализует методы сервиса платежной системы Тинькофф.
    /// </summary>
    public sealed class TinkoffService : ITinkoffService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IConfiguration _configuration;
        private readonly ITinkoffRepository _tinkoffRepository;
        private readonly IServiceProvider _serviceProvider;

        public TinkoffService(PostgreDbContext postgreDbContext, ITinkoffRepository tinkoffRepository, IServiceProvider serviceProvider)
        {
            _postgreDbContext = postgreDbContext;
            _configuration = AutoFac.Resolve<IConfiguration>();
            _tinkoffRepository = tinkoffRepository;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Метод снимет средства с карты покупателя за этап после вычитания комиссии.
        /// </summary>
        /// <param name="orderId">Id заказа в Гаранте.</param>
        /// <param name="amount">Сумма к оплате.</param>
        /// <param name="iterationName">Название итерации этапа.</param>
        /// <returns>Данные платежа с ссылкой на платежную форму.</returns>
        public async Task<PaymentInitOutput> PaymentInitAsync(long orderId, double amount, string iterationName)
        {
            try
            {
                // Преобразует сумму из рублей в копейки.
                var commonService = AutoFac.Resolve<ICommonService>();
                var pennySum = await commonService.ConvertRubToPennyAsync(amount);

                // Найдет userId пользователя, который создал заказ.
                var garantRepository = AutoFac.Resolve<IGarantActionRepository>();
                var gRepository = garantRepository ?? _serviceProvider.GetService<IGarantActionRepository>();

                if (gRepository == null)
                {
                    return new PaymentInitOutput { Success = false };
                }

                var userIdCreatedOrder = await gRepository.GetUserIdCreatedOrderAsync(orderId);

                // Получит Email и номер телефона пользователя, который создал заказ.
                var findUserDataCreatedOrder = await gRepository.FindUserEmailPhoneCreatedOrderAsync(userIdCreatedOrder);

                var sendInitData = new PaymentInitInput
                {
                    TerminalKey = _configuration["TinkoffSandbox:ShopSettings:Id"],
                    Amount = pennySum.ToString(CultureInfo.InvariantCulture),
                    Data = new Data
                    {
                        Email = findUserDataCreatedOrder.Email,
                        Phone = findUserDataCreatedOrder.Phone
                    },
                    Description = $"Оплата этапа {iterationName} на {amount} руб.",
                    OrderId = orderId.ToString(),
                    PayType = "O",
                    Receipt = new Receipt
                    {
                        Email = findUserDataCreatedOrder.Email,
                        Taxation = "osn",
                        Phone = findUserDataCreatedOrder.Phone,
                        Items = new List<Item>
                        {
                            new Item
                            {
                                Name = iterationName,
                                Quantity = 1,
                                Price = pennySum,
                                Amount = pennySum * 1
                            }
                        }
                    }
                };

                // Спишет средства с карты покупателя.
                var initRequest = WebRequest.Create("https://securepay.tinkoff.ru/v2/Init");
                initRequest.Method = "POST";
                initRequest.ContentType = "application/json";
                initRequest.Headers.Add("Authorization", _configuration["TinkoffSandbox:Authorization"]);

                var jsonInitData = JsonConvert.SerializeObject(sendInitData);
                var byteInitData = Encoding.UTF8.GetBytes(jsonInitData);

                // Запишет данные в поток запроса.
                await using var dataInitStream = await initRequest.GetRequestStreamAsync();
                await dataInitStream.WriteAsync(byteInitData, 0, byteInitData.Length);
                HttpWebResponse responseInitData = (HttpWebResponse)await initRequest.GetResponseAsync();

                if (responseInitData.StatusCode != HttpStatusCode.OK)
                {
                    return new PaymentInitOutput { Success = false };
                }

                await using var streamInit = responseInitData.GetResponseStream();

                // Получит результат.
                using var readerInit = new StreamReader(streamInit);
                var initJsonResult = await readerInit.ReadToEndAsync();

                var result = JsonConvert.DeserializeObject<PaymentInitOutput>(initJsonResult);

                if (result == null)
                {
                    return new PaymentInitOutput { Success = false };
                }

                // Обновит систеный Id заказа.
                await _tinkoffRepository.SetSystemOrderIdAsync(orderId, Convert.ToInt64(result.PaymentId));

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }
    }
}
