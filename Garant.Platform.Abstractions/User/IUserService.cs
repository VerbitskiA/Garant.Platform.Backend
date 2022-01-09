using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Footer.Output;
using Garant.Platform.Models.Header.Output;
using Garant.Platform.Models.Suggestion.Output;
using Garant.Platform.Models.Transition.Output;
using Garant.Platform.Models.User.Output;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Abstractions.User
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
        Task<ClaimOutput> CheckAcceptCodeAsync(string code);

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
        /// <param name="account">Аккаунт пользователя.</param>
        /// <returns>Данные пользователя.</returns>
        Task<UserInformationOutput> SaveUserInfoAsync(string firstName, string lastName, string city, string email, string password, string values, int kpp, int bik, string defaultBankName, string account);

        /// <summary>
        /// Метод найдет пользователя по email или номеру телефона.
        /// </summary>
        /// <param name="data">Email или номер телефона.</param>
        /// <returns>Данные пользователя.</returns>
        Task<UserOutput> FindUserByEmailOrPhoneNumberAsync(string data);

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
        /// Метод обновит токен.
        /// </summary>
        /// <returns>Новый токен.</returns>
        Task<ClaimOutput> GenerateTokenAsync();

        /// <summary>
        /// Метод сформирует хлебные крошки для страницы.
        /// </summary>
        /// <param name="selectorPage"> Селектор страницы, для которой нужно сформировать хлебные крошки.</param>
        /// <returns>Список хлебных крошек.</returns>
        Task<IEnumerable<BreadcrumbOutput>> GetBreadcrumbsAsync(string selectorPage);

        /// <summary>
        /// Метод найдет пользователя по коду.
        /// </summary>
        /// <param name="code">Код.</param>
        /// <returns>Id пользователя.</returns>
        Task<string> FindUserByCodeAsync(string code);

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
        /// <param name="account">Логин или почта пользователя.</param>
        /// <returns>Данные перехода.</returns>
        Task<TransitionOutput> GetTransitionWithParamsAsync(long referenceId, string account);

        /// <summary>
        /// Метод получит фио авторизованного пользователя.
        /// </summary>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные пользователя.</returns>
        Task<UserOutput> GetUserFioAsync(string account);

        /// <summary>
        /// Метод проверит, заполнил ил пользователь данные о себе. 
        /// </summary>
        /// <param name="account">Пользователь.</param>
        /// <returns>Флаг проверки.</returns>
        Task<bool> IsWriteProfileDataAsync(string account);

        /// <summary>
        /// Метод сохранит данные формы профиля пользователя.
        /// </summary>
        /// <param name="documentFile">Название документа.</param>
        /// <param name="userInformationInput">Входная модель.</param>
        /// <param name="account">Логин или Email.</param>
        /// <returns>Данные формы.</returns>
        Task<UserInformationOutput> SaveProfileFormAsync(IFormCollection documentFile, string userInformationInput, string account);

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
    }
}
