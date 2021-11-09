using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.FTP.Abstraction;
using Garant.Platform.Models.Business.Input;
using Garant.Platform.Models.Business.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        public BusinessService(PostgreDbContext postgreDbContext, IBusinessRepository businessRepository, IFtpService ftpService)
        {
            _postgreDbContext = postgreDbContext;
            _businessRepository = businessRepository;
            _ftpService = ftpService;
        }

        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        public async Task<CreateUpdateBusinessOutput> CreateUpdateBusinessAsync(IFormCollection businessFilesInput, string businessDataInput, string account)
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

                long lastBusinessId = 1000000;

                // Если в таблице нет записей, то добавленная первая будет иметь id 1000000.
                var count = await _postgreDbContext.Businesses.Select(f => f.BusinessId).CountAsync();

                if (count > 0)
                {
                    // Найдет последний Id бизнеса и увеличит его на 1.
                    lastBusinessId = await _postgreDbContext.Businesses.MaxAsync(c => c.BusinessId);
                    lastBusinessId++;
                }

                // Создаст или обновит бизнес.
                result = await _businessRepository.CreateUpdateBusinessAsync(businessInput, lastBusinessId, businessInput.UrlsBusiness, files, account);

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
        public async Task<IEnumerable<string>> AddTempFilesBeforeCreateBusinessAsync(IFormCollection form, string account)
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
        /// Метод получит франшизу для просмотра или изменения.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные бизнеса.</returns>
        public async Task<BusinessOutput> GetBusinessAsync(long businessId, string mode)
        {
            try
            {
                var result = await _businessRepository.GetBusinessAsync(businessId, mode);

                // Приведет к числу и потом к строке чтобы убрать передние нули, если они будут.
                var newPrice = Convert.ToInt32(result.Price);
                result.Price = newPrice.ToString();

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
    }
}
