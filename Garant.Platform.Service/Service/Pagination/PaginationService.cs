using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Pagination;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Pagination.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Services.Service.Pagination
{
    /// <summary>
    /// Сервис пагинации.
    /// </summary>
    public sealed class PaginationService : IPaginationService
    {
        private readonly PostgreDbContext _postgreDbContext;

        public PaginationService(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод пагинации на ините каталога франшиз.
        /// </summary>
        /// <param name="pageIdx">Номер страницы.</param>
        /// <returns>Данные пагинации.</returns>
        public async Task<IndexOutput> GetInitPaginationCatalogFranchiseAsync(int pageIdx)
        {
            try
            {
                var countRows = 12;   // Кол-во заданий на странице.

                var franchisesList = await _postgreDbContext.Franchises.OrderBy(o => o.FranchiseId).ToListAsync();

                var count = franchisesList.Count;
                //var items = await getTasks.Skip((pageIdx - 1) * countRows).Take(countRows).ToListAsync();
                var items = franchisesList.Take(countRows).ToList();

                var pageData = new PaginationOutput(count, pageIdx, countRows);
                var paginationData = new IndexOutput
                {
                    PageData = pageData,
                    Results = items,
                    TotalCount = count,
                    IsLoadAll = count < countRows,
                    IsVisiblePagination = count > 10,
                    CountAll = count
                };

                if (paginationData.IsLoadAll)
                {
                    var difference = countRows - count;
                    paginationData.TotalCount += difference;
                }

                return paginationData;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получения пагинации франшиз в каталоге.
        /// </summary>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="countRows">Кол-во строк.</param>
        /// <returns>Данные пагинации.</returns>
        public async Task<IndexOutput> GetPaginationCatalogFranchiseAsync(int pageNumber, int countRows)
        {
            try
            {
                var franchisesList = await _postgreDbContext.Franchises.OrderBy(o => o.FranchiseId).ToListAsync();

                var count = franchisesList.Count;
                var items = franchisesList.Skip((pageNumber - 1) * countRows).Take(countRows).ToList();

                var pageData = new PaginationOutput(count, pageNumber, countRows);
                var paginationData = new IndexOutput
                {
                    PageData = pageData,
                    Results = items,
                    TotalCount = count,
                    IsLoadAll = count < countRows,
                    IsVisiblePagination = count > 10,
                    CountAll = count
                };

                if (paginationData.IsLoadAll)
                {
                    var difference = countRows - count;
                    paginationData.TotalCount += difference;
                }

                return paginationData;
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
