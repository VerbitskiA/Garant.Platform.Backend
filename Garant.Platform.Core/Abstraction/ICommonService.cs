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
        /// <param name="number">Номер телефона.</param>
        /// <param name="type">Тип кода. От этого зависит алгоритм создания кода и кол-во цифр.</param>
        /// <returns>Флаг успеха.</returns>
        Task GenerateAcceptCodeAsync(string number, string type);
    }
}
