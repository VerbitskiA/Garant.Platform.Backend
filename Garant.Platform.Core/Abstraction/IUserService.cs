using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Footer.Output;
using Garant.Platform.Models.Header.Output;
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
        /// <param name="role">Роль.</param>
        /// <returns>Данные созданного пользователя.</returns>
        Task<UserOutput> CreateAsync(string name, string lastName, string city, string email, string password, string role);

        /// <summary>
        /// Метод получает список полей основного хидера.
        /// </summary>
        /// <param name="type">Тип хидера, для которого нужно получить список полей.</param>
        /// <returns>Список полей хидера.</returns>
        Task<IEnumerable<HeaderOutput>> InitHeaderAsync(string type);

        /// <summary>
        /// Метод получит список полей футера.
        /// </summary>
        /// <returns>Список полей футера.</returns>
        Task<IEnumerable<FooterOutput>> InitFooterAsync();
    }
}
