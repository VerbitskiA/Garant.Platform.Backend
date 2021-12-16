using System;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Document;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Document.Output;
using Garant.Platform.Models.Entities.Document;
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
                    IsAcceptDocument = false,
                    UserId = userId
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
    }
}
