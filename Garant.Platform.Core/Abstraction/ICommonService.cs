using System.Threading.Tasks;

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
        Task<bool> GenerateAcceptCodeAsync(string data);
    }
}
