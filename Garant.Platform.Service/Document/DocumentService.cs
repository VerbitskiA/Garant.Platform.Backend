using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Core.Data;
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
        /// Метод прикрепит документ к сделке.
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
                        // Запишет документы в БД.
                        result = await _documentRepository.AddVendorDocumentAsync(files.Files[0].FileName, documentInput.DocumentItemId, documentInput.DocumentType, true, account);
                    }
                }

                if (result != null)
                {
                    // Загрузит документы на сервер.
                    await _ftpService.UploadFilesFtpAsync(files.Files);
                }

                return result ?? new DocumentOutput();
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
