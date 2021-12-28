using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.FTP.Abstraction;
using Garant.Platform.Models.Document.Input;
using Garant.Platform.Models.Document.Output;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Garant.Platform.Services.Document
{
    /// <summary>
    /// Сервис реализует методы документов.
    /// </summary>
    public sealed class DocumentService : IDocumentService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IFtpService _ftpService;
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(PostgreDbContext postgreDbContext, IFtpService ftpService, IDocumentRepository documentRepository)
        {
            _postgreDbContext = postgreDbContext;
            _ftpService = ftpService;
            _documentRepository = documentRepository;
        }

        /// <summary>
        ///  TODO: Вынести в бощий метод прикрепления документа если они не будут отличаться. 
        /// Метод прикрепит документ продавца к сделке.
        /// </summary>
        /// <param name="files">Файлы документов.</param>
        /// <param name="documentData">Входная модель.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Данные документов.</returns>
        public async Task<DocumentOutput> AttachmentVendorDocumentDealAsync(IFormCollection files, string documentData, string account)
        {
            try
            {
                DocumentOutput result = null;

                if (files.Files.Any())
                {
                    var documentInput = JsonConvert.DeserializeObject<DocumentInput>(documentData);

                    if (documentInput != null)
                    {
                        if (!documentInput.DocumentType.Equals("DocumentVendor"))
                        {
                            throw new ErrorDocumentTypeException("DocumentVendor");
                        }

                        // Запишет документы в БД.
                        result = await _documentRepository.AddVendorDocumentAsync(files.Files[0].FileName, documentInput.DocumentItemId, documentInput.DocumentType, true, account);
                    }
                }

                if (result != null)
                {
                    // Загрузит документы на сервер.
                    await _ftpService.UploadFilesFtpAsync(files.Files);
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

        /// <summary>
        /// Метод отправит документ основного договора продавца на согласование покупателю.
        /// </summary>
        /// <param name="documentItemId">Id документа.</param>
        /// <param name="isDealDocument">Является ли документом сделки.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <returns>Флаг успеха.</returns>
        public async Task<bool> SendDocumentVendorAsync(long documentItemId, bool isDealDocument, string documentType)
        {
            try
            {
                var result = await _documentRepository.SetSendStatusDocumentVendorAsync(documentItemId, isDealDocument, documentType);

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
        /// Метод отправит документ согласованного покупателем договора продавцу.
        /// </summary>
        /// <param name="documentItemId">Id документа.</param>
        /// <param name="isDealDocument">Является ли документом сделки.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <returns>Флаг успеха.</returns>
        public async Task<bool> SendMainDealDocumentCustomerAsync(long documentItemId, bool isDealDocument, string documentType)
        {
            try
            {
                var result = await _documentRepository.SetSendStatusDocumentCustomerAsync(documentItemId, isDealDocument, documentType);

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
        ///  TODO: Вынести в бощий метод прикрепления документа если они не будут отличаться.
        /// Метод прикрепит документ покупателя к сделке.
        /// </summary>
        /// <param name="files">Файлы документов.</param>
        /// <param name="documentData">Входная модель.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Данные документов.</returns>
        public async Task<DocumentOutput> AttachmentCustomerDocumentDealAsync(IFormCollection files, string documentData, string account)
        {
            try
            {
                DocumentOutput result = null;

                if (files.Files.Any())
                {
                    var documentInput = JsonConvert.DeserializeObject<DocumentInput>(documentData);

                    if (documentInput != null)
                    {
                        if (!documentInput.DocumentType.Equals("DocumentCustomer"))
                        {
                            throw new ErrorDocumentTypeException("DocumentCustomer");
                        }

                        // Запишет документы в БД.
                        result = await _documentRepository.AddCustomerDocumentAsync(files.Files[0].FileName, documentInput.DocumentItemId, documentInput.DocumentType, true, account);
                    }
                }

                if (result != null)
                {
                    // Загрузит документы на сервер.
                    await _ftpService.UploadFilesFtpAsync(files.Files);
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

        /// <summary>
        ///  TODO: Вынести в бощий метод прикрепления документа если они не будут отличаться.
        /// Метод прикрепит акт к сделке.
        /// </summary>
        /// <param name="files">Файлы документов.</param>
        /// <param name="documentData">Входная модель.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Данные документов.</returns>
        public async Task<DocumentOutput> AttachmentActAsync(IFormCollection files, string documentData, string account)
        {
            try
            {
                DocumentOutput result = null;

                if (files.Files.Any())
                {
                    var documentInput = JsonConvert.DeserializeObject<DocumentInput>(documentData);

                    if (documentInput != null)
                    {
                        // Запишет документы в БД.
                        result = await _documentRepository.AddDocumentActAsync(files.Files[0].FileName, documentInput.DocumentItemId, documentInput.DocumentType, true, account);
                    }
                }

                if (result != null)
                {
                    // Загрузит документы на сервер.
                    await _ftpService.UploadFilesFtpAsync(files.Files);
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

        /// <summary>
        /// Метод получит список актов продавца.
        /// </summary>
        /// <param name="documentItemId">Id сделки.</param>
        /// <returns>Список актов продавца.</returns>
        public async Task<IEnumerable<DocumentOutput>> GetVendorActsAsync(long documentItemId)
        {
            try
            {
                var result = await _documentRepository.GetVendorActsAsync(documentItemId);

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
        /// Метод получит список актов покупателя.
        /// </summary>
        /// <param name="documentItemId">Id сделки.</param>
        /// <returns>Список актов покупателя.</returns>
        public async Task<IEnumerable<DocumentOutput>> GetCustomerActsAsync(long documentItemId)
        {
            try
            {
                var result = await _documentRepository.GetCustomerActsAsync(documentItemId);

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
