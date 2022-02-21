using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Franchise.Output;
using System.Linq;
using System.Text.Json;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.FTP.Abstraction;
using Garant.Platform.Models.Franchise.Input;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Garant.Platform.Services.Service.Franchise
{
    public sealed class FranchiseService : IFranchiseService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IFtpService _ftpService;
        private readonly IFranchiseRepository _franchiseRepository;

        public FranchiseService(PostgreDbContext postgreDbContext, IFtpService ftpService,
            IFranchiseRepository franchiseRepository)
        {
            _postgreDbContext = postgreDbContext;
            _ftpService = ftpService;
            _franchiseRepository = franchiseRepository;
        }

        /// <summary>
        /// Метод получит список франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetFranchisesListAsync()
        {
            try
            {
                var items = await _franchiseRepository.GetFranchisesAsync();

                foreach (var item in items)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
                }

                return items;
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
        /// Метод получит список популярных франшиз для главной страницы.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<PopularFranchiseOutput>> GetMainPopularFranchises()
        {
            try
            {
                var result = await _franchiseRepository.GetMainPopularFranchisesList();

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

        /// <summary>
        /// Метод получит 4 франшизы для выгрузки в блок с быстрым поиском.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetFranchiseQuickSearchAsync()
        {
            try
            {
                var items = await _franchiseRepository.GetFranchiseQuickSearchAsync();

                foreach (var item in items)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
                }

                return items;
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
        /// Метод получит список городов франшиз.
        /// </summary>
        /// <returns>Список городов.</returns>
        public async Task<IEnumerable<FranchiseCityOutput>> GetFranchisesCitiesListAsync()
        {
            try
            {
                var result = await _franchiseRepository.GetFranchisesCitiesListAsync();

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

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        public async Task<IEnumerable<CategoryOutput>> GetFranchisesCategoriesListAsync()
        {
            try
            {
                var result = await _franchiseRepository.GetFranchisesCategoriesListAsync();

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

        /// <summary>
        /// Метод получит список видов бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        public async Task<IEnumerable<ViewBusinessOutput>> GetFranchisesViewBusinessListAsync()
        {
            try
            {
                var result = await _franchiseRepository.GetFranchisesViewBusinessListAsync();

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

        /// <summary>
        /// Метод фильтрации франшиз по разным атрибутам.
        /// </summary>
        /// <param name="typeSort">Тип фильтрации цены (по возрастанию или убыванию).</param>
        /// <param name="isGarant">Покупка через гарант.</param>
        /// <param name="minPrice">Прибыль от.</param>
        /// <param name="maxPrice">Прибыль до.</param>
        /// <param name="viewCode"> Код вида бизнеса.</param>
        /// <param name="categoryCode">Код категории.</param>
        /// <param name="minPriceInvest">Сумма инвестиций от.</param>
        /// <param name="maxPriceInvest">Сумма инвестиций до.</param>
        /// <returns>Список франшиз после фильтрации.</returns>
        public async Task<IEnumerable<FranchiseOutput>> FilterFranchisesAsync(string typeSort, double minPrice,
            double maxPrice, string viewCode, string categoryCode, double minPriceInvest, double maxPriceInvest,
            bool isGarant = false)
        {
            try
            {
                var items = await _franchiseRepository.FilterFranchisesAsync(typeSort, minPrice, maxPrice, viewCode,
                    categoryCode, minPriceInvest, maxPriceInvest, isGarant);

                foreach (var item in items)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
                }

                return items;
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
        /// Метод получит новые франшизы, которые были созданы в текущем месяце.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetNewFranchisesAsync()
        {
            try
            {
                var items = await _franchiseRepository.GetNewFranchisesAsync();

                foreach (var item in items)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
                }

                return items;
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
        /// Метод получит список отзывов о франшизах.
        /// </summary>
        /// <returns>Список отзывов.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetReviewsFranchisesAsync()
        {
            try
            {
                var items = await _franchiseRepository.GetReviewsFranchisesAsync();

                foreach (var item in items)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
                }

                return items;
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
        /// Метод создаст новую  или обновит существующую франшизу.
        /// </summary>
        /// <param name="franchiseFilesInput">Входные файлы.</param>
        /// <param name="franchiseDataInput">Данные в строке json.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные франшизы.</returns>
        public async Task<CreateUpdateFranchiseOutput> CreateUpdateFranchiseAsync(IFormCollection franchiseFilesInput,
            string franchiseDataInput, string account)
        {
            try
            {
                var franchiseInput = JsonConvert.DeserializeObject<CreateUpdateFranchiseInput>(franchiseDataInput);
                CreateUpdateFranchiseOutput result = null;

                if (franchiseFilesInput.Files.Any())
                {
                    await _ftpService.UploadFilesFtpAsync(franchiseFilesInput.Files);
                }

                if (franchiseInput == null)
                {
                    return null;
                }

                // Найдет последний Id франшизы и увеличит его на 1.
                var lastFranchiseId = await _postgreDbContext.Franchises.MaxAsync(c => c.FranchiseId);
                lastFranchiseId++;

                // Создаст или обновит франшизу.
                result = await _franchiseRepository.CreateUpdateFranchiseAsync(franchiseInput, lastFranchiseId,
                    franchiseInput.UrlsFranchise, franchiseFilesInput.Files, account);

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
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="franchiseId">Id франшизы.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные франшизы.</returns>
        public async Task<FranchiseOutput> GetFranchiseAsync(long franchiseId, string mode = null)
        {
            try
            {
                var result = await _franchiseRepository.GetFranchiseAsync(franchiseId, mode);

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
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="form">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        public async Task<IEnumerable<string>> AddTempFilesBeforeCreateFranchiseAsync(IFormCollection form,
            string account)
        {
            try
            {
                var files = new FormCollection(null, form.Files).Files;

                // Отправит файлы на FTP-сервер.
                if (files.Any())
                {
                    await _ftpService.UploadFilesFtpAsync(files);
                }

                var results = await _franchiseRepository.AddTempFilesBeforeCreateFranchiseAsync(files, account);

                return results;
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
        /// Метод получит список категорий франшиз.
        /// </summary>
        /// <returns>Список категорий.</returns>
        public async Task<IEnumerable<CategoryOutput>> GetCategoryListAsync()
        {
            try
            {
                var result = await _franchiseRepository.GetCategoryListAsync();

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

        /// <summary>
        /// Метод получит список подкатегорий франшиз.
        /// </summary>
        /// <param name="categoryCode">Код категории, для которой нужно получить список подкатегорий.</param>
        /// <param name="categorySysName">Системное имя категории, для которой нужно получить список подкатегорий.</param>
        /// <returns>Список подкатегорий.</returns>
        public async Task<IEnumerable<SubCategoryOutput>> GetSubCategoryListAsync(string categoryCode,
            string categorySysName)
        {
            try
            {
                if (string.IsNullOrEmpty(categoryCode) || string.IsNullOrEmpty(categorySysName))
                {
                    throw new EmptyParamsFranchiseSubCategoryException();
                }

                var result = await _franchiseRepository.GetSubCategoryListAsync(categoryCode, categorySysName);

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

        /// <summary>
        /// Метод найдет сферы в соответствии с поисковым запросом.
        /// </summary>
        /// <param name="searchText">Поисковый запрос.</param>
        /// <returns>Список сфер.</returns>
        public async Task<IEnumerable<CategoryOutput>> SearchSphereAsync(string searchText)
        {
            try
            {
                var result = await _franchiseRepository.SearchSphereAsync(searchText);

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

        /// <summary>
        /// Метод найдет категории в соответствии с поисковым запросом.
        /// </summary>
        /// <param name="searchText">Поисковый запрос.</param>
        /// <param name="categoryCode">Код сферы.</param>
        /// <param name="categorySysName">Системное название сферы.</param>
        /// <returns>Список категорий.</returns>
        public async Task<IEnumerable<SubCategoryOutput>> SearchCategoryAsync(string searchText, string categoryCode,
            string categorySysName)
        {
            try
            {
                var result = await _franchiseRepository.SearchCategoryAsync(searchText, categoryCode, categorySysName);

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

        /// <summary>
        /// Метод получит список франшиз, которые ожидают согласования.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetNotAcceptedFranchisesAsync()
        {
            try
            {
                var result = await _franchiseRepository.GetNotAcceptedFranchisesAsync();

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