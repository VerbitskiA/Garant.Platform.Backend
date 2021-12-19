using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
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

                // Поищет такой документ в БД и обновит если он уже был добавлен.
                var getDocument = await _postgreDbContext.Documents
                    .AsNoTracking()
                    .Where(d => d.DocumentItemId == documentItemId && d.DocumentType.Equals(documentType))
                    .FirstOrDefaultAsync();

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

                // Обновит документ.
                if (getDocument != null)
                {
                    getDocument.DocumentId = document.DocumentId;

                    _postgreDbContext.Documents.Update(document);
                    await _postgreDbContext.SaveChangesAsync();
                }

                // Добавит новый документ.
                else
                {
                    await _postgreDbContext.Documents.AddAsync(document);
                    await _postgreDbContext.SaveChangesAsync();
                }

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

                // Если такой документ не найден.
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
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// </summary>
        /// <returns>Название документа продавца.</returns>
        public async Task<DocumentOutput> GetAttachmentNameDocumentVendorDealAsync(long documentItemId)
        {
            try
            {
                var result = await _postgreDbContext.Documents
                    .Where(d => d.IsSend.Equals(true) && d.DocumentItemId == documentItemId)
                    .Select(d => new DocumentOutput
                    {
                        DocumentName = d.DocumentName,
                        DocumentId = d.DocumentId
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

        /// <summary>
        /// Метод проверит, подтвердил ли покупатель договор продавца.
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// <param name="account">Аккаунт.</param>
        /// </summary>
        /// <returns>Флаг проверки.</returns>
        public async Task<bool> CheckApproveDocumentVendorAsync(long documentItemId, string account)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                var checkApprove = await _postgreDbContext.Documents
                    .Where(d => d.UserId.Equals(userId) && d.DocumentItemId == documentItemId)
                    .Select(d => d.IsApproveDocument)
                    .FirstOrDefaultAsync();

                return checkApprove ?? false;
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
        /// Метод подтвердит договор продавца.
        /// </summary>
        /// <param name="documentItemId">Id предмета сделки.</param>
        /// <returns>Флаг проверки.</returns>
        public async Task<bool> ApproveDocumentVendorAsync(long documentItemId)
        {
            try
            {
                // Если не передан Id документа предмета сделки.
                if (documentItemId <= 0)
                {
                    throw new EmptyDocumentItemIdException();
                }

                var result = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId && (d.IsSend ?? false))
                    .FirstOrDefaultAsync();

                // Если документ найден, то подтвердит его.
                if (result != null)
                {
                    result.IsApproveDocument = true;
                    await _postgreDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
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
        /// Метод запишет в true флаг отправки согласованного покупателем документа продавца.
        /// </summary>
        /// <param name="documentItemId">Id документа.</param>
        /// <param name="isDealDocument">Является ли документом сделки.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <returns>Флаг успеха.</returns>
        public async Task<bool> SetSendStatusDocumentCustomerAsync(long documentItemId, bool isDealDocument, string documentType)
        {
            try
            {
                var getDocument = await _postgreDbContext.Documents
                    .Where(d => d.DocumentItemId == documentItemId
                                && d.DocumentType.Equals(documentType)
                                && d.IsDealDocument.Equals(isDealDocument))
                    .FirstOrDefaultAsync();

                // Если такой документ не найден.
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
        /// Метод запишет в БД согласованный покупателем договор от продавца.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="documentItemId">Id предмета сделки (франшизы или бизнеса).</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="isDealDocument">Флаг документа сделки.</param>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные документа.</returns>
        public async Task<DocumentOutput> AddCustomerDocumentAsync(string fileName, long documentItemId, string documentType, bool isDealDocument, string account)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                // Поищет такой документ в БД и обновит если он уже был добавлен.
                var getDocument = await _postgreDbContext.Documents
                    .AsNoTracking()
                    .Where(d => d.DocumentItemId == documentItemId && d.DocumentType.Equals(documentType))
                    .FirstOrDefaultAsync();

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

                // Обновит документ.
                if (getDocument != null)
                {
                    document.DocumentId = getDocument.DocumentId;

                    _postgreDbContext.Documents.Update(document);
                    await _postgreDbContext.SaveChangesAsync();
                }

                // Добавит новый документ.
                else
                {
                    await _postgreDbContext.Documents.AddAsync(document);
                    await _postgreDbContext.SaveChangesAsync();
                }

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
    }
}
