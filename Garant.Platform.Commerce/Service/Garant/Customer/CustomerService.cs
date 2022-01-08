using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Commerce.Abstraction.Garant.Customer;
using Garant.Platform.Commerce.Abstraction.Tinkoff;
using Garant.Platform.Commerce.Models.Tinkoff.Input;
using Garant.Platform.Commerce.Models.Tinkoff.Output;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Franchise.Other;
using Newtonsoft.Json;

namespace Garant.Platform.Commerce.Service.Garant.Customer
{
    /// <summary>
    /// Класс реализует методы Гаранта со стороны покупателя.
    /// </summary>
    public sealed class CustomerService : ICustomerService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITinkoffService _tinkoffService;

        public CustomerService(PostgreDbContext postgreDbContex, IUserRepository userRepository, ICustomerRepository customerRepository, ITinkoffService tinkoffService)
        {
            _postgreDbContext = postgreDbContex;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _tinkoffService = tinkoffService;
        }

        /// <summary>
        /// Метод холдирует платеж на определенный срок, пока не получит подтверждения оплаты.
        /// </summary>
        /// <param name="originalId">Id франшизы или бизнеса.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="orderType">Тип заказа.</param>
        /// <param name="iteration">Номер итерации этапа.</param>
        /// <returns>Данные платежа.</returns>
        public async Task<PaymentInitOutput> PaymentIterationCustomerAsync(long originalId, string account, string orderType, int iteration)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);
                double amount = 0;
                var i = 0;
                var iterationName = string.Empty;

                if (iteration == 1)
                {
                    i = 0;
                }

                // От текущей даты прибавит 14 дней как срок холдирования платежа.
                var endDate = DateTime.Now.AddDays(14);

                // Найдет франшизу или бизнес.
                if (orderType.Equals("Franchise"))
                {
                    var franchiseService = AutoFac.Resolve<IFranchiseService>();
                    var franchise = await franchiseService.GetFranchiseAsync(originalId);

                    if (franchise != null)
                    {
                        // Найдет этапы итерации (что входит в инвестиции) франшизы.
                        var franchiseData = JsonConvert.DeserializeObject<List<ParseInvestInclude>>(franchise.InvestInclude)[i];

                        // Выберет цену итерации для оплаты.
                        amount = Convert.ToInt64(franchiseData.Price);

                        // Выберет название предмета итерации.
                        iterationName = franchiseData.Name;
                    }
                }

                if (orderType.Equals("Business"))
                {
                    var businessService = AutoFac.Resolve<IBusinessService>();
                    var business = await businessService.GetBusinessAsync(originalId);

                    if (business != null)
                    {
                        // Найдет этапы итерации (что входит в инвестиции) бизнеса.
                        var businessData = JsonConvert.DeserializeObject<List<ParseInvestInclude>>(business.InvestPrice)[i];

                        // Выберет цену итерации для оплаты.
                        amount = Convert.ToInt64(businessData.Price);

                        // Выберет название предмета итерации.
                        iterationName = businessData.Name;
                    }
                }

                if (amount <= 0)
                {
                    return new PaymentInitOutput { Success = false };
                }

                if (string.IsNullOrEmpty(iterationName))
                {
                    return new PaymentInitOutput { Success = false };
                }

                // Объект с описанием платежа.
                var description = new Description
                {
                    Short = iterationName,
                    Full = iterationName
                };

                var newOrder = await _customerRepository.CreateOrderAsync(originalId, amount, description, orderType, userId, iteration);

                if (newOrder == null)
                {
                    return new PaymentInitOutput { Success = false };
                }

                // Если заказ в сервисе Гарант создан успешно, то создаст платеж в системе банка и вернет ссылку на платежную форму.
                var result = await _tinkoffService.PaymentInitAsync(newOrder.OrderId, amount, iterationName, originalId);

                return result.Success ? result : new PaymentInitOutput { Success = false };
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
