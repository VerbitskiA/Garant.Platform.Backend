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
using Garant.Platform.Commerce.Core.Exceptions;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Commerce.Models.Tinkoff.Output;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Microsoft.Extensions.Configuration;
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
        private readonly IGarantActionRepository _garantActionRepository;

        public TinkoffService(PostgreDbContext postgreDbContext, ITinkoffRepository tinkoffRepository, IGarantActionRepository garantActionRepository)
        {
            _postgreDbContext = postgreDbContext;
            _configuration = AutoFac.Resolve<IConfiguration>();
            _tinkoffRepository = tinkoffRepository;
            _garantActionRepository = garantActionRepository;
        }

        /// <summary>
        /// Метод снимет средства с карты покупателя за этап после вычитания комиссии.
        /// </summary>
        /// <param name="orderId">Id заказа в Гаранте.</param>
        /// <param name="amount">Сумма к оплате.</param>
        /// <param name="iterationName">Название итерации этапа.</param>
        /// <returns>Данные платежа с ссылкой на платежную форму.</returns>
        public async Task<PaymentInitOutput> PaymentInitAsync(long orderId, double amount, string iterationName, long dealItemId)
        {
            try
            {
                // Преобразует сумму из рублей в копейки.
                var commonService = AutoFac.Resolve<ICommonService>();
                var pennySum = await commonService.ConvertRubToPennyAsync(amount);

                // Найдет userId пользователя, который создал заказ.
                var garantRepository = AutoFac.Resolve<IGarantActionRepository>();
                var userIdCreatedOrder = await garantRepository.GetUserIdCreatedOrderAsync(orderId);

                // Получит Email и номер телефона пользователя, который создал заказ.
                var findUserDataCreatedOrder = await garantRepository.FindUserEmailPhoneCreatedOrderAsync(userIdCreatedOrder);

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
                //await _tinkoffRepository.SetOrderStatusByIdAsync(orderId, Convert.ToInt64(result.PaymentId));

                // Проверит статус созданного платежа.
                await GetStatePaymentAsync(result.PaymentId, orderId);

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

        /// <summary>
        /// Метод получит статус платежей итерации этапа в система банка.
        /// </summary>
        /// <param name="statuses">Массив с данными для проверки статусов платежей в системе банка.</param>
        /// <returns></returns>
        public async Task GetStateAllPaymentAsync(IReadOnlyList<StatePaymentInput> statuses)
        {
            try
            {
                foreach (var item in statuses)
                {
                    if (string.IsNullOrEmpty(item.PaymentId) && Convert.ToInt64(item.PaymentId) <= 0)
                    {
                        throw new EmptyOrderIdException("Не передан Id платежа в системе банка.");
                    }

                    var sendData = new GetStatePaymentInput
                    {
                        PaymentId = item.PaymentId,
                        TerminalKey = _configuration["TinkoffSandbox:ShopSettings:Id"],
                        Token = _configuration["TinkoffSandbox:Authorization"]
                    };

                    // Проверит статус платежа в системе банка.
                    var request = WebRequest.Create("https://securepay.tinkoff.ru/v2/GetState");
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Headers.Add("Authorization", _configuration["TinkoffSandbox:Authorization"]);

                    var json = JsonConvert.SerializeObject(sendData);
                    var byteData = Encoding.UTF8.GetBytes(json);

                    // Запишет данные в поток запроса.
                    await using var stream = await request.GetRequestStreamAsync();
                    await stream.WriteAsync(byteData, 0, byteData.Length);
                    HttpWebResponse responseInitData = (HttpWebResponse)await request.GetResponseAsync();

                    //if (responseInitData.StatusCode != HttpStatusCode.OK)
                    //{
                    //    return new PaymentInitOutput { Success = false };
                    //}

                    await using var streamResult = responseInitData.GetResponseStream();

                    // Получит результат.
                    using var reader = new StreamReader(streamResult);
                    var jsonResult = await reader.ReadToEndAsync();
                    Console.WriteLine();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        public async Task GetStatePaymentAsync(string paymentId, long orderId)
        {
            try
            {
                if (string.IsNullOrEmpty(paymentId) && Convert.ToInt64(paymentId) <= 0)
                {
                    throw new EmptyOrderIdException("Не передан Id платежа в системе банка.");
                }

                var commonService = AutoFac.Resolve<ICommonService>();

                // Готовит объект значений для хэша в SHA-256.
                var hashValues = new Dictionary<string, object>
                {
                    {
                        "TerminalKey", _configuration["TinkoffSandbox:ShopSettings:Id"]
                    },

                    {
                        "PaymentId", paymentId
                    },

                    {
                        "Password", _configuration["TinkoffSandbox:ShopSettings:Name"]
                    }
                };

                var hash = await commonService.HashSha256Async(hashValues);

                var sendData = new GetStatePaymentInput
                {
                    PaymentId = paymentId,
                    TerminalKey = _configuration["TinkoffSandbox:ShopSettings:Id"],
                    Token = hash
                };

                // Получит старый статус заказа.
                var garantRepository = AutoFac.Resolve<IGarantActionRepository>();
                var currentOrderStatus = await garantRepository.GetOrderByIdAsync(orderId);

                // Проверит статус платежа в системе банка.
                var request = WebRequest.Create("https://securepay.tinkoff.ru/v2/GetState");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", _configuration["TinkoffSandbox:Authorization"]);

                var json = JsonConvert.SerializeObject(sendData);
                var byteData = Encoding.UTF8.GetBytes(json);

                // Запишет данные в поток запроса.
                await using var stream = await request.GetRequestStreamAsync();
                await stream.WriteAsync(byteData, 0, byteData.Length);
                HttpWebResponse responseInitData = (HttpWebResponse)await request.GetResponseAsync();

                if (responseInitData.StatusCode != HttpStatusCode.OK)
                {
                    return;
                }

                await using var streamResult = responseInitData.GetResponseStream();

                // Получит результат.
                using var reader = new StreamReader(streamResult);
                var jsonResult = await reader.ReadToEndAsync();

                // Запишет новый статус заказа.
                var checkStatus = JsonConvert.DeserializeObject<GetPaymentStatusOutput>(jsonResult);

                // Если статусы заказа разные.
                if (!currentOrderStatus.OrderStatus.Equals(checkStatus.Status))
                {
                    // Запишет новый статус заказа.
                    await _tinkoffRepository.SetOrderStatusByIdAsync(orderId, checkStatus.Status);
                }
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
