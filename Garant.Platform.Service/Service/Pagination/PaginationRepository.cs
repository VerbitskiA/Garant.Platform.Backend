using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Pagination;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Business.Output;
using Garant.Platform.Models.Franchise.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Services.Service.Pagination
{
    /// <summary>
    /// Репозиторий пагинации.
    /// </summary>
    public class PaginationRepository : IPaginationRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly ICommonService _commonService;

        public PaginationRepository(PostgreDbContext postgreDbContext, ICommonService commonService)
        {
            _commonService = commonService;
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод получит список бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        public async Task<List<BusinessOutput>> GetBusinessListAsync()
        {
            try
            {
                var result = await _postgreDbContext.Businesses
                    .OrderBy(o => o.BusinessId)
                    .Select(b => new BusinessOutput
                    {
                        ActivityDetail = b.ActivityDetail,
                        ActivityPhotoName = b.ActivityPhotoName,
                        Address = b.Address,
                        Assets = b.Assets,
                        AssetsPhotoName = b.AssetsPhotoName,
                        BusinessAge = b.BusinessAge,
                        BusinessId = b.BusinessId,
                        BusinessName = b.BusinessName,
                        EmployeeCountYear = b.EmployeeCountYear,
                        Form = b.Form,
                        Status = b.Status,
                        Price = string.Format("{0:0,0}", b.Price),
                        UrlsBusiness = b.UrlsBusiness,
                        TurnPrice = b.TurnPrice,
                        ProfitPrice = b.ProfitPrice,
                        Payback = b.Payback,
                        Profitability = b.Profitability,
                        InvestPrice = b.InvestPrice,
                        Text = b.Text,
                        Share = b.Share,
                        Site = b.Site,
                        Peculiarity = b.Peculiarity,
                        NameFinModelFile = b.NameFinModelFile,
                        ReasonsSale = b.ReasonsSale,
                        ReasonsSalePhotoName = b.ReasonsSalePhotoName,
                        UrlVideo = b.UrlVideo,
                        IsGarant = b.IsGarant,
                        UserId = b.UserId,
                        DateCreate = b.DateCreate,
                        Category = b.Category,
                        SubCategory = b.SubCategory
                    })
                    .ToListAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        public async Task<List<BusinessOutput>> GetBusinessesListIsGarantAsync()
        {
            try
            {
                //выбираются по умолчанию бизнес с флагом IsGarant == true
                var businessList = await _postgreDbContext.Businesses.Where(o => o.IsGarant)
                    .Select(f => new BusinessOutput
                    {
                        DateCreate = f.DateCreate,
                        Price = string.Format("{0:0,0}", f.Price),
                        CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                        DayDeclination = "дня",
                        Text = f.Text,
                        TextDoPrice = f.TextDoPrice,
                        BusinessName = f.BusinessName,
                        Url = f.UrlsBusiness,
                        IsGarant = f.IsGarant,
                        ProfitPrice = f.ProfitPrice,
                        TotalInvest = string.Format("{0:0,0}", f.ProfitPrice),
                        BusinessId = f.BusinessId
                    }).ToListAsync();

                foreach (var item in businessList)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                }

                return businessList;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        public async Task<List<FranchiseOutput>> GetFranchisesListIsGarantAsync()
        {
            try
            {
                var franchisesList = await _postgreDbContext.Franchises.Where(o => o.IsGarant)
                   .Select(f => new FranchiseOutput
                   {
                       DateCreate = f.DateCreate,
                       Price = string.Format("{0:0,0}", f.Price),
                       CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                       DayDeclination = "дня",
                       Text = f.Text,
                       TextDoPrice = f.TextDoPrice,
                       Title = f.Title,
                       Url = f.Url,
                       IsGarant = f.IsGarant,
                       ProfitPrice = f.ProfitPrice,
                       TotalInvest = string.Format("{0:0,0}", f.GeneralInvest),
                       FranchiseId = f.FranchiseId
                   }).ToListAsync();

                foreach (var item in franchisesList)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);                    
                }

                return franchisesList;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }
    }
}
