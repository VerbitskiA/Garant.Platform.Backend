using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Configurator.Models.Output;
using Garant.Platform.Models.Configurator.Output;
using Garant.Platform.Models.User.Output;

namespace Garant.Platform.Configurator.Abstractions
{
    /// <summary>
    /// Абстракция сервиса конфигуратора.
    /// </summary>
    public interface IConfiguratorService
    {
        /// <summary>
        /// Метод заведет нового сотрудника сервиса.
        /// </summary>
        /// <param name="employeeRoleName">Название роли сотрдника.</param>
        /// <param name="employeeRoleSystemName">Системное название роли сотрудника.</param>
        /// <param name="employeeStatus">Статус сотрудника.</param>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="patronymic">Отчество.</param>
        /// <param name="phoneNumber">Номер телефона.</param>
        /// <param name="email">Почта сотрудника.</param>
        /// <param name="telegramTag">Тэг в телеграме.</param>
        /// <returns>Данные добавленного сотрудника</returns>
        Task<CreateEmployeeOutput> CreateEmployeeAsync(string employeeRoleName, string employeeRoleSystemName,
            string employeeStatus, string firstName, string lastName, string patronymic, string phoneNumber,
            string email, string telegramTag);

        /// <summary>
        /// Метод получит список меню конфигуратора.
        /// </summary>
        /// <returns>Список меню.</returns>
        Task<IEnumerable<ConfiguratorMenuOutput>> GetMenuItemsAsync();

        /// <summary>
        /// Метод авторизует сотрудника сервиса.
        /// </summary>
        /// <param name="inputData">Email или телефон.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Данные сотрудника.</returns>
        Task<ConfiguratorLoginOutput> ConfiguratorLoginAsync(string inputData, string password);

        /// <summary>
        /// Метод получит список действий при работе с блогами.
        /// </summary>
        /// <returns>Список действий.</returns>
        Task<IEnumerable<BlogActionOutput>> GetBlogActionsAsync();

        /// <summary>
        /// Метод утвердит карточку. После этого карточка попадает в каталоги.
        /// </summary>
        /// <param name="cardId">Id карточки.</param>
        /// <param name="cardType">Тип карточки.</param>
        /// <returns>Статус утверждения.</returns>
        Task<bool> AcceptCardAsync(long cardId, string cardType);

        /// <summary>
        /// Метод отклонит публикацию карточки. 
        /// </summary>
        /// <param name="cardId">Id карточки.</param>
        /// <param name="cardType">Тип карточки.</param>
        /// <param name="comment">Комментарий отклонения.</param>
        /// <returns>Статус отклонения.</returns>
        Task<bool> RejectCardAsync(long cardId, string cardType, string comment);

        /// <summary>
        /// Метод создаст новую сферу.
        /// </summary>
        /// <param name="sphereName">Название сферы.</param>
        /// <param name="sphereType">Тип сферы (бизнес или франшиза).</param>
        /// <param name="sysName">Системное название сферы.</param>
        /// <returns>Созданная сфера.</returns>
        Task<CreateSphereOutput> CreateSphereAsync(string sphereName, string sphereType, string sysName);

        /// <summary>
        /// Метод создаст категорию сферы.
        /// </summary>
        /// <param name="sphereCode">Код сферы (guid).</param>
        /// <param name="categoryName">Название категории.</param>
        /// <param name="categoryType">Тип категории.</param>
        /// <param name="sysName">Системное название.</param>
        /// <returns>Созданная категория.</returns>
        Task<CreateCategoryOutput> CreateCategoryAsync(string sphereCode, string categoryName, string categoryType, string sysName);
    }
}