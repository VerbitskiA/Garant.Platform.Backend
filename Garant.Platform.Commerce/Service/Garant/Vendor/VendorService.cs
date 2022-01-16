using System;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Commerce.Abstraction.Garant.Vendor;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Commerce.Output;

namespace Garant.Platform.Commerce.Service.Garant.Vendor
{
    /// <summary>
    /// Класс реализует методы Гаранта со стороны продавца.
    /// </summary>
    public sealed class VendorService : IVendorService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IVendorRepository _vendorRepository;

        public VendorService(PostgreDbContext postgreDbContext, IVendorRepository vendorRepository)
        {
            _postgreDbContext = postgreDbContext;
            _vendorRepository = vendorRepository;
        }

        /// <summary>
        /// Метод подтверждает начало сделки. Продавец подтверждает продажу.
        /// </summary>
        /// <param name="itemDealId">Id предмета сделки (франшизы или бизнеса).</param>
        /// <param name="orderType">Тип предмета сделки (франшиза или бизнес).</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Флаг успеха.</returns>
        public async Task<DealOutput> AcceptActualDealAsync(long itemDealId, string orderType, string account)
        {
            try
            {
                var isCheck = false;

                if (orderType.Equals("Franchise"))
                {
                    var franchiseService = AutoFac.Resolve<IFranchiseService>();
                    var checkFranchie = await franchiseService.GetFranchiseAsync(itemDealId);

                    if (checkFranchie != null)
                    {
                        isCheck = true;
                    }
                }

                if (orderType.Equals("Business"))
                {
                    var businessService = AutoFac.Resolve<IBusinessService>();
                    var checkBusiness = await businessService.GetBusinessAsync(itemDealId);

                    if (checkBusiness != null)
                    {
                        isCheck = true;
                    }
                }

                if (isCheck)
                {
                    return null;
                }

                var userService = AutoFac.Resolve<IUserRepository>();
                var userId = await userService.FindUserIdUniverseAsync(account);

                // Проверит существование открытой сделки, чтобы не дублировать ее.
                var checkDeal = await _vendorRepository.CheckDealByItemDealIdAsync(itemDealId, account);

                if (!checkDeal)
                {
                    return null;
                }

                var result = await _vendorRepository.CreateDealAsync(itemDealId, userId);

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
