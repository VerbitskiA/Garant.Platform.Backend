using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.FTP.Abstraction;
using Garant.Platform.Mailings.Abstraction;
using Garant.Platform.Messaging.Abstraction.Notifications;
using Garant.Platform.Messaging.Consts;
using Garant.Platform.Messaging.Enums;
using Garant.Platform.Models.Business.Input;
using Garant.Platform.Models.Business.Output;
using Garant.Platform.Models.Pagination.Output;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Services.Service.Business
{
    /// <summary>
    /// Сервис готового бизнеса.
    /// </summary>
    public sealed class BusinessService : IBusinessService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IBusinessRepository _businessRepository;
        private readonly IFtpService _ftpService;
        private readonly INotificationsService _notificationsService;
        private readonly IUserRepository _userRepository;
        private readonly INotificationsRepository _notificationsRepository;

        public BusinessService(IBusinessRepository businessRepository,
            IFtpService ftpService,
            INotificationsService notificationsService,
            IUserRepository userRepository,
            INotificationsRepository notificationsRepository)
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
            _businessRepository = businessRepository;
            _ftpService = ftpService;
            _notificationsService = notificationsService;
            _userRepository = userRepository;
            _notificationsRepository = notificationsRepository;
        }

        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        public async Task<CreateUpdateBusinessOutput> CreateUpdateBusinessAsync(IFormCollection businessFilesInput,
            string businessDataInput, string account)
        {
            try
            {
                var files = new FormCollection(null, businessFilesInput.Files).Files;
                var businessInput = JsonSerializer.Deserialize<CreateUpdateBusinessInput>(businessDataInput);
                CreateUpdateBusinessOutput result = null;

                if (files.Any())
                {
                    await _ftpService.UploadFilesFtpAsync(files);
                }

                if (businessInput == null)
                {
                    return null;
                }

                // Создаст или обновит бизнес.
                result = await _businessRepository.CreateUpdateBusinessAsync(businessInput,
                    businessInput.UrlsBusiness, files, account);

                // Сформирует ссылку на карточку франшизы.
                var commonRepository = AutoFac.Resolve<ICommonRepository>();
                var cardUrl = await commonRepository.GetCardUrlAsync("ModerationBusinessCard");
                var newUrl = cardUrl + result.BusinessId;

                // Отправит оповещение администрации сервиса.
                var mailService = AutoFac.Resolve<IMailingService>();
                await mailService.SendMailAfterCreateCardAsync("Бизнес", newUrl);

                // Отправит уведомление о модерации карточки через SignalR.
                await _notificationsService.SendCardModerationAsync();

                var userId = await _userRepository.FindUserIdUniverseAsync(account);
                var userInfo = await _userRepository.GetUserProfileInfoByIdAsync(userId);

                // Запишет уведомление в БД.
                await _notificationsRepository.SaveNotifyAsync("AfterCreateCardNotify", NotifyMessage.CARD_MODERATION_TITLE, NotifyMessage.CARD_MODERATION_TEXT, NotificationLevelEnum.Success.ToString(), true, userId, "AfterCreateCard");

                var userEmail = string.Empty;

                if (userInfo != null)
                {
                    userEmail = userInfo.Email;
                }

                // Отправит пользователю на почту уведомление о созданной карточке.
                await mailService.SendMailUserAfterCreateCardAsync(userEmail, "Бизнес", newUrl);

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
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="form">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        public async Task<IEnumerable<string>> AddTempFilesBeforeCreateBusinessAsync(IFormCollection form,
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

                var results = await _businessRepository.AddTempFilesBeforeCreateBusinessAsync(files, account);

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
        /// Метод получит бизнес для просмотра или изменения.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные бизнеса.</returns>
        public async Task<BusinessOutput> GetBusinessAsync(long businessId, string mode = null)
        {
            try
            {
                var result = await _businessRepository.GetBusinessAsync(businessId, mode);

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
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        public async Task<IEnumerable<GetBusinessCategoryOutput>> GetBusinessCategoriesAsync()
        {
            try
            {
                var result = await _businessRepository.GetBusinessCategoriesAsync();

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
        /// Метод получит список подкатегорий бизнеса.
        /// </summary>
        /// <returns>Список подкатегорий.</returns>
        public async Task<IEnumerable<BusinessSubCategoryOutput>> GetSubBusinessCategoryListAsync()
        {
            try
            {
                var result = await _businessRepository.GetSubBusinessCategoryListAsync();

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
        /// Метод получит список городов.
        /// </summary>
        /// <returns>Список городов.</returns>
        public async Task<IEnumerable<BusinessCitiesOutput>> GetCitiesListAsync()
        {
            try
            {
                var result = await _businessRepository.GetCitiesListAsync();

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
        /// Метод получит список популярного бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        public async Task<IEnumerable<PopularBusinessOutput>> GetPopularBusinessAsync()
        {
            try
            {
                var result = await _businessRepository.GetPopularBusinessAsync();

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
        /// Метод получит список бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        public async Task<IEnumerable<BusinessOutput>> GetBusinessListAsync()
        {
            try
            {
                var result = await _businessRepository.GetBusinessListAsync();

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
        /// Метод фильтрует список бизнесов по параметрам.
        /// </summary>
        /// <param name="typeSortPrice">Тип сортировки цены (убыванию, возрастанию).</param>
        /// <param name="profitMinPrice">Цена от.</param>
        /// <param name="profitMaxPrice">Цена до.</param>
        /// <param name="categoryCode">Город.</param>
        /// <param name="viewCode">Код вида бизнеса.</param>
        /// <param name="minPriceInvest">Сумма общих инвестиций от.</param>
        /// <param name="maxPriceInvest">Сумма общих инвестиций до.</param>
        /// <param name="isGarant">Флаг гаранта.</param>
        /// <returns>Список бизнесов после фильтрации.</returns>
        public async Task<IEnumerable<BusinessOutput>> FilterBusinessesAsync(string typeSortPrice,
            double profitMinPrice, double profitMaxPrice, string viewCode, string categoryCode, double minPriceInvest,
            double maxPriceInvest, bool isGarant = false)
        {
            try
            {
                var result = await _businessRepository.FilterBusinessesAsync(typeSortPrice, profitMinPrice,
                    profitMaxPrice, viewCode, categoryCode, minPriceInvest, maxPriceInvest, isGarant);

                foreach (var item in result)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
                }

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
        /// Метод получит новый бизнес, который был создан в текущем месяце.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        public async Task<IEnumerable<BusinessOutput>> GetNewBusinesseListAsync()
        {
            try
            {
                var result = await _businessRepository.GetNewBusinesseListAsync();

                foreach (var item in result)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
                }

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
        /// Метод получит список бизнесов, которые ожидают согласования.
        /// </summary>
        /// <returns>Список бизнесов.</returns>
        public async Task<IEnumerable<BusinessOutput>> GetNotAcceptedBusinessesAsync()
        {
            try
            {
                var result = await _businessRepository.GetNotAcceptedBusinessesAsync();

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
        /// Метод фильтрует бизнес по параметрам с учётом пагинации.
        /// </summary>
        /// <param name="typeSortPrice">Тип сортировки цены (убыванию, возрастанию).</param>
        /// <param name="minPrice">Цена от.</param>
        /// <param name="maxPrice">Цена до.</param>
        /// <param name="city">Город.</param>
        /// <param name="categoryCode">Код вида бизнеса.</param>
        /// <param name="profitMinPrice">Прибыль в месяц от.</param>
        /// <param name="profitMaxPrice">прибыль в месяц до.</param>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="countRows">Количество записей.</param>
        /// <param name="isGarant">Флаг гаранта.</param>
        /// <returns>Список бизнесов после фильтрации и данные для пагинации.</returns>
        public async Task<IndexOutput> FilterBusinessesWithPaginationAsync(string typeSortPrice, double minPrice,
            double maxPrice,
            string city, string categoryCode, double profitMinPrice,
            double profitMaxPrice, int pageNumber, int countRows, bool isGarant = true)
        {
            try
            {
                var businessList = await _businessRepository.FilterBusinessesIndependentlyAsync(typeSortPrice, minPrice,
                    maxPrice,
                    city, categoryCode, profitMinPrice,
                    profitMaxPrice, isGarant);

                foreach (var item in businessList)
                {
                    item.FullText = item.Text + " " + item.CountDays + " " + item.DayDeclination;
                }

                var count = businessList.Count;
                var items = businessList.Skip((pageNumber - 1) * countRows).Take(countRows).ToList();

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
        /// Метод поместит бизнес в архив.
        /// </summary>
        /// <param name="businessId">Идентификатор бизнеса.</param>
        /// <returns>Статус архивации.</returns>
        public async Task<bool> ArchiveBusinessAsync(long businessId)
        {
            try
            {
                var result = await _businessRepository.ArchiveBusinessAsync(businessId);

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
        /// Метод вернёт список бизнесов из архива.
        /// </summary>
        /// <returns>Список архивированных бизнесов.</returns>
        public async Task<IEnumerable<BusinessOutput>> GetArchiveBusinessListAsync()
        {
            try
            {
                var result = await _businessRepository.GetArchiveBusinessListAsync();

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
        /// Метод восстановит бизнес из архива.
        /// </summary>
        /// <param name="businessId">Идентификатор бизнеса.</param>
        /// <returns>Статус восстановления бизнеса.</returns>
        public async Task<bool> RestoreBusinessFromArchive(long businessId)
        {
            try
            {
                var result = await _businessRepository.RestoreBusinessFromArchive(businessId);

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
        /// Метод удалит из архива бизнесы, которые там находятся больше одного месяца (>=31 дней).
        /// </summary>
        /// <returns>Бизнесы в архиве после удаления.</returns>
        public async Task<IEnumerable<BusinessOutput>> RemoveBusinessesOlderMonthFromArchiveAsync()
        {
            try
            {
                var result = await _businessRepository.RemoveBusinessesOlderMonthFromArchiveAsync();

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