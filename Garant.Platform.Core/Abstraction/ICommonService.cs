using System.Threading.Tasks;
using Garant.Platform.Models.Mailing.Output;

namespace Garant.Platform.Core.Abstraction
{
    /// <summary>
    /// Абстракция общего сервиса.
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// Метод создает код. Кол-во цифр зависит от переданного типа.
        /// </summary>
        /// <param name="data">Телефон или почта.</param>
        /// <returns>Флаг успеха.</returns>
        Task<MailngOutput> GenerateAcceptCodeAsync(string data);

        /// <summary>
        /// Метод хэширует пароль аналогично как это делает Identity.
        /// </summary>
        /// <param name="password">Исходный пароль без хэша.</param>
        /// <returns>Хэш пароля.</returns>
        Task<string> HashPasswordAsync(string password);
    }
}
