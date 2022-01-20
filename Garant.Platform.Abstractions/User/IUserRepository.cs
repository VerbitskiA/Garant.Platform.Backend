using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.Footer.Output;
using Garant.Platform.Models.Header.Output;
using Garant.Platform.Models.Suggestion.Output;
using Garant.Platform.Models.Transition.Output;
using Garant.Platform.Models.User.Input;
using Garant.Platform.Models.User.Output;

namespace Garant.Platform.Abstractions.User
{
    /// <summary>
    /// Абстракция репозитория пользователя для работы с БД.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Метод найдет пользователя по email или номеру телефона.
        /// </summary>
        /// <param name="data">Email или номер телефона.</param>
        /// <returns>Данные пользователя.</returns>
        Task<UserOutput> FindUserByEmailOrPhoneNumberAsync(string data);

        /// <summary>
        /// Метод найдет пользователя по коду.
        /// </summary>
        /// <param name="data">Параметр поиска.</param>
        /// <returns>Id пользователя.</returns>
        Task<string> FindUserByCodeAsync(string data);

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

        /// <summary>
        /// Метод завершит регистрацию и добавит информацию о пользователе.
        /// </summary>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="city">Город.</param>
        /// <param name="email">Email.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="values">Причины регистрации разделенные запятой.</param>
        /// <param name="kpp">КПП.</param>
        /// <param name="bik">БИК.</param>
        /// <param name="defaultBankName">Название банка которое нужно сохранить по умолчанию.</param>
        /// <param name="corrAccountNumber">Корреспондентский счёт банка получателя.</param>
        /// <returns>Данные пользователя.</returns>
        Task<UserInformationOutput> SaveUserInfoAsync(string firstName, string lastName, string city, string email,
            string password, string values, string guid, string kpp, string bik, string defaultBankName, string corrAccountNumber, string inn);

        /// <summary>
        /// Метод найдет захэшированный пароль пользователя по логину или email или номеру телефона.
        /// </summary>
        /// <param name="data">Логин или email пользователя.</param>
        /// <returns>Захэшированный пароль.</returns>
        Task<string> GetUserHashPasswordAsync(string data);

        /// <summary>
        /// Метод подтвердит почту по временному коду (guid).
        /// </summary>
        /// <param name="code">Временный код.</param>
        /// <returns>Флаг подтверждения.</returns>
        Task<bool> ConfirmEmailAsync(string code);

        /// <summary>
        /// Метод получит одно предложение с флагом IsSingle.
        /// </summary>
        /// <param name="isSingle">Получить одно предложение.</param>
        /// <param name="isAll">Получить все предложения.</param>
        /// <returns>Данные предложения.</returns>
        Task<SuggestionOutput> GetSingleSuggestion(bool isSingle, bool isAll);

        /// <summary>
        /// Метод получит все предложения с флагом IsAll.
        /// </summary>
        /// <param name="isSingle">Получить одно предложение.</param>
        /// <param name="isAll">Получить все предложения.</param>
        /// <returns>Список предложений.</returns>
        Task<IEnumerable<SuggestionOutput>> GetAllSuggestionsAsync(bool isSingle, bool isAll);

        /// <summary>
        /// Метод сформирует хлебные крошки для страницы.
        /// </summary>
        /// <param name="selectorPage"> Селектор страницы, для которой нужно сформировать хлебные крошки.</param>
        /// <returns>Список хлебных крошек.</returns>
        Task<List<BreadcrumbOutput>> GetBreadcrumbsAsync(string selectorPage);

        /// <summary>
        /// Метод запишет переход пользователя.
        /// </summary>
        /// <param name="account">Логин или почта пользователя.</param>
        /// <param name="transitionType">Тип перехода.</param>
        /// <param name="referenceId">Id франшизы или готового бизнеса.</param>
        /// <param name="otherId">Id другого пользователя.</param>
        /// <param name="typeItem">Тип предмета обсуждения.</param>
        /// <returns>Флаг записи перехода.</returns>
        Task<bool> SetTransitionAsync(string account, string transitionType, long referenceId, string otherId, string typeItem);

        /// <summary>
        /// Метод получит переход пользователя.
        /// </summary>
        /// <param name="account">Логин или почта пользователя.</param>
        /// <returns>Данные перехода.</returns>
        Task<TransitionOutput> GetTransitionAsync(string account);

        /// <summary>
        /// Метод получит переход пользователя по параметрам.
        /// </summary>
        /// <param name="referenceId">Id заказа или предмета сделки.</param>
        /// <param name="otherId">Id заказа в системе банка.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Данные перехода.</returns>
        Task<TransitionOutput> GetTransitionWithParamsAsync(long refrenceId, string orerId, string userId);

        /// <summary>
        /// Метод получит фио авторизованного пользователя.
        /// </summary>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные пользователя.</returns>
        Task<UserOutput> GetUserFioAsync(string account);

        /// <summary>
        /// Метод универсально найдет Id пользователя.
        /// </summary>
        /// <param name="account">Данные для авторизации.</param>
        /// <returns>Id пользователя.</returns>
        Task<string> FindUserIdUniverseAsync(string account);

        /// <summary>
        /// Метод проверит, заполнил ил пользователь данные о себе. 
        /// </summary>
        /// <param name="account">Пользователь.</param>
        /// <returns>Флаг проверки.</returns>
        Task<bool> IsWriteProfileDataAsync(string account);

        /// <summary>
        /// Метод сохранит данные формы профиля пользователя.
        /// </summary>
        /// <param name="userInformationInput">Входная модель.</param>
        /// <param name="account">Логин или Email.</param>
        /// <param name="documentName">Название документа.</param>
        /// <returns>Данные формы.</returns>
        Task<UserInformationOutput> SaveProfileFormAsync(UserInformationInput userInformationInput, string account, string documentName);

        /// <summary>
        /// Метод найдет все данные пользователя.
        /// </summary>
        /// <param name="account"></param>
        /// <returns>Данные пользователя.</returns>
        Task<UserEntity> FindUserUniverseAsync(string account);

        /// <summary>
        /// Метод получит информацию профиля пользователя.
        /// </summary>
        /// <param name="account">Логин или email пользователя.</param>
        /// <returns>Данные профиля.</returns>
        Task<UserInformationOutput> GetProfileInfoAsync(string account);

        /// <summary>
        /// Метод получит список меню для пунктов ЛК.
        /// </summary>
        /// <returns>Список пунктов ЛК.</returns>
        Task<IEnumerable<ProfileNavigationOutput>> GetProfileMenuListAsync();

        /// <summary>
        /// Метод получит информацию профиля пользователя по его Id.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Данные профиля.</returns>
        Task<UserInformationOutput> GetUserProfileInfoByIdAsync(string userId);

        /// <summary>
        /// Проверит, подтверждал ли пользователь свою почту, если нет, то попросит подтвердить.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Статус проверки.</returns>
        Task<bool> CheckConfirmEmailAsync(string userId);
    }
}
