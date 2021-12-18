using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Document.Output;
using Garant.Platform.Models.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Garant.Platform.Services.Document
{
    /// <summary>
    /// Репозитория документов.
    /// </summary>
    public sealed class DocumentRepository : IDocumentRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IUserRepository _userRepository;

        public DocumentRepository(PostgreDbContext postgreDbContext, IUserRepository userRepository)
        {
            _postgreDbContext = postgreDbContext;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Метод запишет в БД договор от продавца.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="documentItemId">Id предмета сделки (франшизы или бизнеса).</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="isDealDocument">Флаг документа сделки.</param>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные документа.</returns>
        public async Task<DocumentOutput> AddVendorDocumentAsync(string fileName, long documentItemId, string documentType, bool isDealDocument, string account)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                var document = new DocumentEntity
                {
                    DateCreate = DateTime.Now,
                    DocumentItemId = documentItemId,
                    DocumentName = fileName,
                    DocumentType = documentType,
                    IsDealDocument = isDealDocument,
                    IsApproveDocument = false,
                    IsRejectDocument = false,
                    UserId = userId,
                    IsSend = false
                };
                await _postgreDbContext.Documents.AddAsync(document);
                await _postgreDbContext.SaveChangesAsync();

                var jsonString = JsonConvert.SerializeObject(document);
                var result = JsonConvert.DeserializeObject<DocumentOutput>(jsonString);

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
        /// Метод запишет в true флаг отправки документа на согласование покупателю.
        /// </summary>
        /// <param name="documentItemId">Id документа.</param>
        /// <param name="isDealDocument">Является ли документом сделки.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <returns>Флаг успеха.</returns>
        public async Task<bool> SetSendStatusDocumentVendorAsync(long documentItemId, bool isDealDocument, string documentType)
        {
            try
            {
                var getDocument = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId
                                && d.DocumentType.Equals(documentType)
                                && d.IsDealDocument.Equals(isDealDocument))
                    .FirstOrDefaultAsync();

                if (getDocument == null)
                {
                    return false;
                }

                getDocument.IsSend = true;

                await _postgreDbContext.SaveChangesAsync();

                return true;
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
        /// Метод получит название документа, который отправлен на согласование покупателю.
        /// <param name="account">Аккаунт.</param>
        /// </summary>
        /// <returns>Название документа.</returns>
        public async Task<DocumentOutput> GetAttachmentNameDocumentDealAsync(string account)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                var result = await _postgreDbContext.Documents
                    .Where(d => d.UserId.Equals(userId) && d.IsSend.Equals(true))
                    .Select(d => new DocumentOutput
                    {
                        DocumentName = d.DocumentName
                    })
                    .FirstOrDefaultAsync();

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
