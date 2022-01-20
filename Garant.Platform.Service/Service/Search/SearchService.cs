using System;
using System.Collections;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.Search;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;

namespace Garant.Platform.Services.Service.Search
{
    /// <summary>
    /// Сервис поиска.
    /// </summary>
    public sealed class SearchService : ISearchService
    {
        private readonly IFranchiseRepository _franchiseRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly PostgreDbContext _postgreDbContext;

        public SearchService(IFranchiseRepository franchiseRepository, IBusinessRepository businessRepository, PostgreDbContext postgreDbContext)
        {
            _franchiseRepository = franchiseRepository;
            _businessRepository = businessRepository;
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод найдет данные по запросу.
        /// </summary>
        /// <param name="searchType">Тип поиска.</param>
        /// <param name="searchText">Текст поиска.</param>
        /// <returns>Список с результатами.</returns>
        public async Task<IEnumerable> SearchAsync(string searchType, string searchText)
        {
            try
            {
                IEnumerable result = null;

                if (!string.IsNullOrEmpty(searchText))
                {
                    // Уберет пробелы с конца и с начала.
                    searchText = searchText.Trim();
                }

                if (searchType.Equals("franchise"))
                {
                    result = await _franchiseRepository.SearchByFranchisesAsync(searchText);
                }

                if (searchType.Equals("business"))
                {
                    result = await _businessRepository.SearchByBusinessesAsync(searchText);
                }

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
