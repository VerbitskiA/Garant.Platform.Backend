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
        /// Метод прикрепит документ к сделке.
        /// </summary>
        /// <param name="files">Файлы документов.</param>
        /// <param name="documentData">Входная модель.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Данные документов.</returns>
        Task<DocumentOutput> AttachmentVendorDocumentDealAsync(IFormCollection files, string documentData, string account);
    }
}
