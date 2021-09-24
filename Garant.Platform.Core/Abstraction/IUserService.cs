using System.Threading.Tasks;
using Garant.Platform.Models.User.Output;

namespace Garant.Platform.Core.Abstraction
{
    /// <summary>
    /// Абстракция сервиса пользователей.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Метод авторизует пользователя стандартным способом.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Данные авторизованного пользователя.</returns>
        Task<ClaimOutput> LoginAsync(string email, string password);

        /// <summary>
        /// Метод проверит правильность кода подтверждения.
        /// </summary>
        /// <param name="code">Код подтверждения.</param>
        /// <returns>Статус проверки.</returns>
        Task<bool> CheckAcceptCodeAsync(string code);

        /// <summary>
        /// Метод создаст нового пользователя.
        /// </summary>
        /// <param name="name">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="city">Город.</param>
        /// <param name="email">Email.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Данные созданного пользователя.</returns>
        Task<UserOutput> CreateAsync(string name, string lastName, string city, string email, string password);
    }
}
