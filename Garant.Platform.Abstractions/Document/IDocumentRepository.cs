using System.Threading.Tasks;
using Garant.Platform.Models.Document.Output;

namespace Garant.Platform.Abstractions.Document
{
    /// <summary>
    /// Абстракция репозитория документов для работы с БД.
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>
        /// Метод запишет в БД договор от продавца.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="documentItemId">Id предмета сделки (франшизы или бизнеса).</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="isDealDocument">Флаг документа сделки.</param>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные документа.</returns>
        Task<DocumentOutput> AddVendorDocumentAsync(string fileName, long documentItemId, string documentType, bool isDealDocument, string account);
    }
}
