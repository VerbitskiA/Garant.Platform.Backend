using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.FTP.Abstraction;
using Garant.Platform.Mailings.Abstraction;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.Footer.Output;
using Garant.Platform.Models.Header.Output;
using Garant.Platform.Models.Suggestion.Output;
using Garant.Platform.Models.Transition.Output;
using Garant.Platform.Models.User.Input;
using Garant.Platform.Models.User.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Garant.Platform.Services.Service.User
{
    /// <summary>
    /// Сервис пользователя.
    /// </summary>
    public sealed class UserService : IUserService
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IMailingService _mailingService;
        private readonly IUserRepository _userRepository;
        private readonly IFtpService _ftpService;
        private readonly ICommonService _commonService;

        public UserService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, PostgreDbContext postgreDbContext, IMailingService mailingService, IUserRepository userRepository, IFtpService ftpService, ICommonService commonService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _postgreDbContext = postgreDbContext;
            _mailingService = mailingService;
            _userRepository = userRepository;
            _ftpService = ftpService;
            _commonService = commonService;
        }

        /// <summary>
        /// Метод авторизует пользователя стандартным способом.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Данные авторизованного пользователя.</returns>
        public async Task<ClaimOutput> LoginAsync(string email, string password)
        {
            try
            {
                var auth = await _signInManager.PasswordSignInAsync(email, password, false, false);

                if (auth == null || !auth.Succeeded)
                {
                    throw new ErrorUserAuthorize(email);
                }

                var claim = GetIdentityClaim(email);

                // Генерит токен юзеру.
                var token = GenerateToken(claim).Result;

                // Проверит, заполнял ли пользователь данные о себе.
                var isWrite = await IsWriteProfileDataAsync(email);

                var result = new ClaimOutput
                {
                    User = email,
                    Token = token,
                    IsSuccess = true,
                    IsWriteProfileData = isWrite
                };

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод выдает токен пользователю, если он прошел авторизацию.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>Токен пользователя.</returns>
        private ClaimsIdentity GetIdentityClaim(string email)
        {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email)
                //new Claim(JwtRegisteredClaimNames.UniqueName, username)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }

        /// <summary>
        /// Метод генерит токен юзеру.
        /// </summary>
        /// <param name="claimsIdentity">Объект полномочий.</param>
        /// <returns>Строку токена.</returns>
        private Task<string> GenerateToken(ClaimsIdentity claimsIdentity)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Task.FromResult(encodedJwt);
        }

        /// <summary>
        /// Метод проверит правильность кода подтверждения.
        /// </summary>
        /// <param name="code">Код подтверждения.</param>
        /// <returns>Статус проверки.</returns>
        public async Task<ClaimOutput> CheckAcceptCodeAsync(string code)
        {
            try
            {
                var user = await (from u in _postgreDbContext.Users
                                  where u.Code.Equals(code)
                                  select u)
                    .FirstOrDefaultAsync();

                var claim = GetIdentityClaim(code);

                // Генерит токен юзеру.
                var token = GenerateToken(claim).Result;

                ClaimOutput result = null;

                // Если пользователь найден.
                if (user != null)
                {
                    // Проверит, заполнял ли пользователь данные о себе.
                    var isWrite = await IsWriteProfileDataAsync(user.Email ?? user.PhoneNumber ?? code);

                    result = new ClaimOutput
                    {
                        Token = token,
                        User = user.PhoneNumber ?? user.Email,
                        IsSuccess = true,
                        IsWriteProfileData = isWrite
                    };
                }

                else
                {
                    result = new ClaimOutput
                    {
                        IsSuccess = false
                    };
                }

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

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
        public async Task<UserOutput> CreateAsync(string name, string lastName, string city, string email, string password, string role)
        {
            try
            {
                var date = DateTime.UtcNow;

                // Создаст пользователя.
                await _userManager.CreateAsync(new UserEntity
                {
                    UserName = email,
                    FirstName = name,
                    LastName = lastName,
                    Email = email,
                    City = city,
                    DateRegister = date,
                    Code = string.Empty,
                    UserPassword = password,
                    LockoutEnabled = false,
                    UserRole = role
                }, password);

                var reult = new UserOutput
                {
                    Email = email,
                    City = city,
                    FirstName = name,
                    LastName = lastName,
                    DateRegister = date
                };

                return reult;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод получает список полей основного хидера.
        /// </summary>
        /// <param name="type">Тип хидера, для которого нужно получить список полей.</param>
        /// <returns>Список полей хидера.</returns>
        public async Task<IEnumerable<HeaderOutput>> InitHeaderAsync(string type)
        {
            try
            {
                var result = await _userRepository.InitHeaderAsync(type);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод получит список полей футера.
        /// </summary>
        /// <returns>Список полей футера.</returns>
        public async Task<IEnumerable<FooterOutput>> InitFooterAsync()
        {
            try
            {
                var result = await _userRepository.InitFooterAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод завершит регистрацию и добавит информацию о пользователе.
        /// </summary>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="city">Город.</param>
        /// <param name="email">Email.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="values">Причины регистрации разделенные запятой.</param>
        /// <returns>Данные пользователя.</returns>
        public async Task<UserInformationOutput> SaveUserInfoAsync(string firstName, string lastName, string city, string email, string password, string values)
        {
            try
            {
                if (string.IsNullOrEmpty(firstName)
                    || string.IsNullOrEmpty(lastName)
                    || string.IsNullOrEmpty(city)
                    || string.IsNullOrEmpty(email)
                    || string.IsNullOrEmpty(password)
                    || string.IsNullOrEmpty(values))
                {
                    throw new EmptyUserInformationException();
                }

                // Генерит guid код для подтверждения почты.
                var guid = Guid.NewGuid().ToString();

                var result = await _userRepository.SaveUserInfoAsync(firstName, lastName, city, email, password, values, guid);

                if (result == null)
                {
                    return null;
                }

                // Отправит подтверждение на почту.
                await _mailingService.SendAcceptEmailAsync(email, $"Подтвердите регистрацию, перейдя по ссылке: <a href='https://gobizy.ru?code={guid}'>Подтвердить</a>", "Gobizy: Подтверждение почты");
                //http://localhost:4200

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод найдет пользователя по email или номеру телефона.
        /// </summary>
        /// <param name="data">Email или номер телефона.</param>
        /// <returns>Данные пользователя.</returns>
        public async Task<UserOutput> FindUserByEmailOrPhoneNumberAsync(string data)
        {
            try
            {
                var result = await _userRepository.FindUserByEmailOrPhoneNumberAsync(data);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод подтвердит почту по временному коду (guid).
        /// </summary>
        /// <param name="code">Временный код.</param>
        /// <returns>Флаг подтверждения.</returns>
        public async Task<bool> ConfirmEmailAsync(string code)
        {
            try
            {
                var result = await _userRepository.ConfirmEmailAsync(code);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит одно предложение с флагом IsSingle.
        /// </summary>
        /// <param name="isSingle">Получить одно предложение.</param>
        /// <param name="isAll">Получить все предложения.</param>
        /// <returns>Данные предложения.</returns>
        public async Task<SuggestionOutput> GetSingleSuggestion(bool isSingle, bool isAll)
        {
            try
            {
                var result = await _userRepository.GetSingleSuggestion(isSingle, isAll);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит все предложения с флагом IsAll.
        /// </summary>
        /// <param name="isSingle">Получить одно предложение.</param>
        /// <param name="isAll">Получить все предложения.</param>
        /// <returns>Список предложений.</returns>
        public async Task<IEnumerable<SuggestionOutput>> GetAllSuggestionsAsync(bool isSingle, bool isAll)
        {
            try
            {
                var result = await _userRepository.GetAllSuggestionsAsync(isSingle, isAll);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод обновит токен.
        /// </summary>
        /// <returns>Новый токен.</returns>
        public async Task<ClaimOutput> GenerateTokenAsync()
        {
            try
            {
                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: new ClaimsIdentity().Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var result = new ClaimOutput
                {
                    Token = encodedJwt
                };

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод сформирует хлебные крошки для страницы.
        /// </summary>
        /// <param name="selectorPage"> Селектор страницы, для которой нужно сформировать хлебные крошки.</param>
        /// <returns>Список хлебных крошек.</returns>
        public async Task<IEnumerable<BreadcrumbOutput>> GetBreadcrumbsAsync(string selectorPage)
        {
            try
            {
                var result = await _userRepository.GetBreadcrumbsAsync(selectorPage);

                // Вычислит последний активный пункт цепочки хлебных крошек.
                var maxPosition = result.Max(l => l.Position);

                foreach (var item in result)
                {
                    if (item.Position == maxPosition)
                    {
                        item.IsCurrent = true;
                    }
                }

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод найдет пользователя по коду.
        /// </summary>
        /// <param name="data">Параметр поиска.</param>
        /// <returns>Id пользователя.</returns>
        public async Task<string> FindUserByCodeAsync(string data)
        {
            try
            {
                var result = await _userRepository.FindUserByCodeAsync(data);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод запишет переход пользователя.
        /// </summary>
        /// <param name="account">Логин или почта пользователя.</param>
        /// <param name="transitionType">Тип перехода.</param>
        /// <param name="referenceId">Id франшизы или готового бизнеса.</param>
        /// <param name="otherId">Id другого пользователя.</param>
        /// <param name="typeItem">Тип предмета обсуждения.</param>
        /// <returns>Флаг записи перехода.</returns>
        public async Task<bool> SetTransitionAsync(string account, string transitionType, long referenceId, string otherId, string typeItem)
        {
            try
            {
                var result = await _userRepository.SetTransitionAsync(account, transitionType, referenceId, otherId, typeItem);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит переход пользователя.
        /// </summary>
        /// <param name="account">Логин или почта пользователя.</param>
        /// <returns>Данные перехода.</returns>
        public async Task<TransitionOutput> GetTransitionAsync(string account)
        {
            try
            {
                var result = await _userRepository.GetTransitionAsync(account);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод получит фио авторизованного пользователя.
        /// </summary>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные пользователя.</returns>
        public async Task<UserOutput> GetUserFioAsync(string account)
        {
            try
            {
                var result = await _userRepository.GetUserFioAsync(account);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод проверит, заполнил ил пользователь данные о себе. 
        /// </summary>
        /// <param name="account">Пользователь.</param>
        /// <returns>Флаг проверки.</returns>
        public async Task<bool> IsWriteProfileDataAsync(string account)
        {
            try
            {
                var result = await _userRepository.IsWriteProfileDataAsync(account);

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод сохранит данные формы профиля пользователя.
        /// </summary>
        /// <param name="documentFile">Название документа.</param>
        /// <param name="userInformationInput">Входная модель.</param>
        /// <param name="account">Логин или Email.</param>
        /// <returns>Данные формы.</returns>
        public async Task<UserInformationOutput> SaveProfileFormAsync(IFormCollection documentFile, string userInformationJson, string account)
        {
            try
            {
                UserInformationOutput result = null;
                var fileName = string.Empty;

                // Загрузит документ на сервер.
                if (documentFile.Files.Any())
                {
                    await _ftpService.UploadFilesFtpAsync(documentFile.Files);
                    fileName = documentFile.Files[0].FileName;
                }

                var userInformationInput = JsonSerializer.Deserialize<UserInformationInput>(userInformationJson);

                if (userInformationInput != null)
                {
                    result = await _userRepository.SaveProfileFormAsync(userInformationInput, account, fileName);
                }

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод получит информацию профиля пользователя.
        /// </summary>
        /// <param name="account">Логин или email пользователя.</param>
        /// <returns>Данные профиля.</returns>
        public async Task<UserInformationOutput> GetProfileInfoAsync(string account)
        {
            try
            {
                var result = await _userRepository.GetProfileInfoAsync(account);

                if (result != null)
                {
                    var user = await _userRepository.FindUserUniverseAsync(account);

                    result.Password = null;

                    // Вычислит кол-во времени на сайте.
                    if (user != null)
                    {
                        // Вычислит года.
                        var calcYear = DateTime.Now.Year - user.DateRegister.Year;

                        // Вычислит месяцы.
                        var calcMonth = await _commonService.GetSubtractMonthAsync(user.DateRegister, DateTime.Now);

                        if (calcYear == 0)
                        {
                            result.CountTimeSite = calcMonth.ToString(CultureInfo.InvariantCulture) + " мес.";
                        }

                        else
                        {
                            // Если 12 мес, то прибавит 1 год.
                            if ((int)calcMonth == 12)
                            {
                                calcYear++;
                                result.CountTimeSite = calcYear + " г.";
                            }

                            else
                            {
                                result.CountTimeSite = calcYear + " г." + calcMonth + " мес.";
                            }
                        }

                        // Вычислит кол-во объявлений пользователя.
                        var userId = await _userRepository.FindUserIdUniverseAsync(account);

                        if (!string.IsNullOrEmpty(userId))
                        {
                            // Вычислит кол-во опубликованных объявлений франшиз + кол-во опубликованного бизнеса.
                            // Кол-во франшиз.
                            var countFranchises = await _postgreDbContext.Franchises
                                .Where(f => f.UserId.Equals(userId))
                                .CountAsync();

                            //// Кол-во бизнеса.
                            var countBusinesses = await _postgreDbContext.Businesses
                                .Where(f => f.UserId.Equals(userId))
                                .CountAsync();

                            // Всего.
                            var countAd = countFranchises + countBusinesses;
                            result.CountAd = countAd;
                        }
                    }
                }

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод получит список меню для пунктов ЛК.
        /// </summary>
        /// <returns>Список пунктов ЛК.</returns>
        public async Task<IEnumerable<ProfileNavigationOutput>> GetProfileMenuListAsync()
        {
            try
            {
                var result = await _userRepository.GetProfileMenuListAsync();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }
    }
}
