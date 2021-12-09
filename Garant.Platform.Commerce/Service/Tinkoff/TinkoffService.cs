using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Garant.Platform.Commerce.Abstraction.Tinkoff;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Commerce.Models.Tinkoff.Output;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Microsoft.Extensions.Configuration;

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

        public TinkoffService(PostgreDbContext postgreDbContext, ITinkoffRepository tinkoffRepository)
        {
            _postgreDbContext = postgreDbContext;
            _configuration = AutoFac.Resolve<IConfiguration>();
            _tinkoffRepository = tinkoffRepository;
        }

        /// <summary>
        /// Метод холдирует платеж на определенный срок, пока не получит подтверждения оплаты.
        /// </summary>
        /// <param name="orderId">Id заказа в Гаранте.</param>
        /// <param name="amount">Сумма к оплате.</param>
        /// <param name="endDate">Дата действия холдирования.</param>
        /// <param name="description">Объект с описанием.</param>
        /// <param name="redirectUrl">Ссылка редиректа после успешного холдирования.</param>
        /// <returns>Данные холдирования платежа.</returns>
        public async Task<HoldPaymentOutput> HoldPaymentAsync(long orderId, double amount, DateTime endDate, Description description, string redirectUrl)
        {
            try
            {
                var request = WebRequest.Create("https://business.tinkoff.ru/openapi/sandbox/api/v1/e-invoice");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", _configuration["TinkoffSandbox:Authorization"]);

                // Получит ссылку на оплату.
                var link = await _tinkoffRepository.GetReturnForPaymentUrlAsync();

                var sendData = new HoldPaymentInput
                {
                    Amount = amount,
                    Description = new Description
                    {
                        Short = description.Short,
                        Full = description.Full
                    },
                    EndDate = endDate.ToString("o"),
                    OrderId = orderId.ToString(),
                    RedirectUrl = link,
                    Shop = new Shop
                    {
                        Id = _configuration["TinkoffSandbox:ShopSettings:Id"],
                        Name = _configuration["TinkoffSandbox:ShopSettings:Name"]
                    }
                };

                await using var streamWriter = new StreamWriter(await request.GetRequestStreamAsync());
                string json = JsonSerializer.Serialize(sendData);
                await streamWriter.WriteAsync(json);

                var httpResponse = await request.GetResponseAsync();
                using var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = await streamReader.ReadToEndAsync();

                return null;
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
