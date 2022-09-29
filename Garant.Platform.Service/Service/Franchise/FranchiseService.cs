using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Franchise.Output;
using System.Linq;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Abstractions.Franchise;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Utils;
using Garant.Platform.FTP.Abstraction;
using Garant.Platform.Mailings.Abstraction;
using Garant.Platform.Messaging.Abstraction.Notifications;
using Garant.Platform.Messaging.Consts;
using Garant.Platform.Messaging.Enums;
using Garant.Platform.Models.Franchise.Input;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Garant.Platform.Models.Pagination.Output;

namespace Garant.Platform.Services.Service.Franchise
{
    public sealed class FranchiseService : IFranchiseService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IFtpService _ftpService;
        private readonly IFranchiseRepository _franchiseRepository;
        private readonly INotificationsService _notificationsService;
        private readonly IUserRepository _userRepository;
        private readonly INotificationsRepository _notificationsRepository;

        public FranchiseService(IFtpService ftpService,
            IFranchiseRepository franchiseRepository,
            INotificationsService notificationsService,
            IUserRepository userRepository,
            INotificationsRepository notificationsRepository)
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
            _ftpService = ftpService;
            _franchiseRepository = franchiseRepository;
            _notificationsService = notificationsService;
            _userRepository = userRepository;
            _notificationsRepository = notificationsRepository;
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
                    return new CreateUpdateFranchiseOutput();
                }

                // Создаст или обновит франшизу.
                result = await _franchiseRepository.CreateUpdateFranchiseAsync(franchiseInput,
                    franchiseInput.UrlsFranchise, franchiseFilesInput.Files, account);
                
                // Сформирует ссылку на карточку франшизы.
                var commonRepository = AutoFac.Resolve<ICommonRepository>();
                var cardUrl = await commonRepository.GetCardUrlAsync("ModerationFranchiseCard");
                var newUrl = cardUrl + result.FranchiseId;
                
                // Отправит оповещение администрации сервиса.
                var mailService = AutoFac.Resolve<IMailingService>();
                await mailService.SendMailAfterCreateCardAsync("Франшиза", newUrl);
                
                var userId = await _userRepository.FindUserIdUniverseAsync(account);
                var userInfo = await _userRepository.GetUserProfileInfoByIdAsync(userId);
                
                // Отправит уведомление о модерации карточки через SignalR.
                await _notificationsService.SendCardModerationAsync();
                
                // Запишет уведомление в БД.
                await _notificationsRepository.SaveNotifyAsync("AfterCreateCardNotify", NotifyMessage.CARD_MODERATION_TITLE, NotifyMessage.CARD_MODERATION_TEXT, NotificationLevelEnum.Success.ToString(), true, userId, "AfterCreateCard");

                var userEmail = string.Empty;

                if (userInfo != null)
                {
                    userEmail = userInfo.Email;
                }
                
                // Отправит пользователю на почту уведомление о созданной карточке.
                await mailService.SendMailUserAfterCreateCardAsync(userEmail, "Франшиза", newUrl);

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
        /// Метод фильтрует франшизы с учётом пагинации.
        /// </summary>
        /// <param name="typeSort">Тип сортировки цены (по возрастанию или убыванию).</param>
        /// <param name="viewCode">Код вида бизнеса.</param>
        /// <param name="categoryCode">Код категории.</param>
        /// <param name="minInvest">Сумма инвестиций от.</param>
        /// <param name="maxInvest">Сумма инвестиций до.</param>
        /// <param name="minProfit">Прибыль от.</param>
        /// <param name="maxProfit">Прибыль до.</param>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="countRows">Количество объектов.</param>
        /// <param name="isGarant">Покупка через гарант.</param>
        /// <returns>Список франшиз после фильтрации и данные для пагинации.</returns>
        public async Task<IndexOutput> FilterFranchisesWithPaginationAsync(string typeSort, string viewCode,
            string categoryCode, double minInvest, double maxInvest, double minProfit, double maxProfit, int pageNumber,
            int countRows, bool isGarant = true)
        {
            try
            {
                var franchisesList = await _franchiseRepository.FilterFranchisesIndependentlyAsync(typeSort, viewCode,
                    categoryCode,
                    minInvest, maxInvest, minProfit,
                    maxProfit, pageNumber, countRows, isGarant);


                foreach (var item in franchisesList)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
                }

                var count = franchisesList.Count;
                var items = franchisesList.Skip((pageNumber - 1) * countRows).Take(countRows).ToList();

                var pageData = new PaginationOutput(count, pageNumber, countRows);
                var paginationData = new IndexOutput
                {
                    PageData = pageData,
                    Results = items,
                    TotalCount = count,
                    IsLoadAll = count < countRows,
                    IsVisiblePagination = count > countRows,
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

        /// <summary>
        /// Метод поместит франшизу в архив.
        /// </summary>
        /// <param name="franchiseId">Идентификатор франшизы.</param>
        /// <returns>Статус архивации.</returns>
        public async Task<bool> ArchiveFranchiseAsync(long franchiseId)
        {
            try
            {
                var result = await _franchiseRepository.ArchiveFranchiseAsync(franchiseId);

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
        /// Метод вернёт список франшиз из архива.
        /// </summary>
        /// <returns>Список архивированных франшиз.</returns>
        public async Task<IEnumerable<FranchiseOutput>> GetArchiveFranchiseListAsync()
        {
            try
            {
                var result = await _franchiseRepository.GetArchiveFranchiseListAsync();

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
        /// Метод восстановит франшизу из архива.
        /// </summary>
        /// <param name="franchiseId">Идентификатор франшизы.</param>
        /// <returns>Статус восстановления франшизы.</returns>
        public async Task<bool> RestoreFranchiseFromArchive(long franchiseId)
        {
            try
            {
                var result = await _franchiseRepository.RestoreFranchiseFromArchive(franchiseId);

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
        /// Метод удалит из архива франшизы, которые там находятся больше одного месяца (>=31 дней).
        /// </summary>
        /// <returns>Франшизы в архиве после удаления.</returns>
        public async Task<IEnumerable<FranchiseOutput>> RemoveFranchisesOlderMonthFromArchiveAsync()
        {
            try
            {
                var result = await _franchiseRepository.RemoveFranchisesOlderMonthFromArchiveAsync();

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