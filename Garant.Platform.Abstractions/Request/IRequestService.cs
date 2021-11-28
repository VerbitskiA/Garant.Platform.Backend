using System.Threading.Tasks;
using Garant.Platform.Models.Request.Output;

namespace Garant.Platform.Abstractions.Request
{
    /// <summary>
    /// Абстракция сервиса заявок.
    /// </summary>
    public interface IRequestService
    {
        /// <summary>
        /// Метод создаст заявку франшизы.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="phone">Телефон.</param>
        /// <param name="city">Город.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="franchiseId">Id франшизы, по которой оставлена заявка.</param>
        /// <returns>Данные заявки.</returns>
        Task<RequestFranchiseOutput> CreateRequestFranchiseAsync(string userName, string phone, string city, string account, long franchiseId);
    }
}
