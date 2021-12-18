using System.Threading.Tasks;
using Garant.Platform.Models.Document.Output;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Abstractions.Document
{
    /// <summary>
    /// Абстракция сервиса документов.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Метод прикрепит документ продавца к сделке.
        /// </summary>
        /// <param name="files">Файлы документов.</param>
        /// <param name="documentData">Входная модель.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Данные документов.</returns>
        Task<DocumentOutput> AttachmentVendorDocumentDealAsync(IFormCollection files, string documentData, string account);

        /// <summary>
        /// Метод отправит документ основного договора продавца на согласование покупателю.
        /// </summary>
        /// <param name="documentItemId">Id документа.</param>
        /// <param name="isDealDocument">Является ли документом сделки.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <returns>Флаг успеха.</returns>
        Task<bool> SendDocumentVendorAsync(long documentItemId, bool isDealDocument, string documentType);
    }
}
