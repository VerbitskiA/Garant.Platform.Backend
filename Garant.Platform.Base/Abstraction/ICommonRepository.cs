using System.Threading.Tasks;

namespace Garant.Platform.Base.Abstraction
{
    /// <summary>
    /// Общий репозиторий для работы с БД.
    /// </summary>
    public interface ICommonRepository
    {
        /// <summary>
        /// Метод получит ссылку для формирования.
        /// </summary>
        /// <param name="cardType">Тип карточки.</param>
        /// <returns>Строка url.</returns>
        Task<string> GetCardUrlAsync(string cardType);
    }
}