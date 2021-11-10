using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Pagination;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Business.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Services.Service.Pagination
{
    /// <summary>
    /// Репозиторий пагинации.
    /// </summary>
    public class PaginationRepository : IPaginationRepository
    {
        private readonly PostgreDbContext _postgreDbContext;

        public PaginationRepository(PostgreDbContext postgreDbContext)
        {
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
    }
}
