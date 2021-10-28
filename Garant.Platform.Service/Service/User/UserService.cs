using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Mailings.Abstraction;
using Garant.Platform.Models.Entities.Transition;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.Footer.Output;
using Garant.Platform.Models.Header.Output;
using Garant.Platform.Models.Suggestion.Output;
using Garant.Platform.Models.Transition.Output;
using Garant.Platform.Models.User.Output;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Garant.Platform.Service.Service.User
{
    /// <summary>
    /// Сервис пользователя.
    /// </summary>
    public sealed class UserService : IUserService
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly PostgreDbContext _postgreDbContext;
        private readonly ICommonService _commonService;
        private readonly IMailingService _mailingService;
        //private readonly IFranchiseService _franchiseService;

        public UserService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, PostgreDbContext postgreDbContext, ICommonService commonService, IMailingService mailingService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _postgreDbContext = postgreDbContext;
            _commonService = commonService;
            _mailingService = mailingService;
            //_franchiseService = franchiseService;
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

                var result = new ClaimOutput
                {
                    User = email,
                    Token = token,
                    IsSuccess = true
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
                    result = new ClaimOutput
                    {
                        Token = token,
                        User = user.PhoneNumber ?? user.Email,
                        IsSuccess = true
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
                var result = await (from h in _postgreDbContext.Headers
                                    where h.HeaderType.Equals(type)
                                    orderby h.Position
                                    select new HeaderOutput
                                    {
                                        Name = h.HeaderName,
                                        Type = h.HeaderType,
                                        Position = h.Position
                                    })
                    .ToListAsync();

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
                var result = await (from f in _postgreDbContext.Footers
                                    orderby f.Column
                                    select new FooterOutput
                                    {
                                        Title = f.FooterTitle,
                                        Name = f.FooterFieldName,
                                        IsPlace = f.IsPlace,
                                        IsSignleTitle = f.IsSignleTitle,
                                        Column = f.Column,
                                        Position = f.Position
                                    })
                    .ToListAsync();

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
                var result = new UserInformationOutput();

                if (string.IsNullOrEmpty(firstName)
                    || string.IsNullOrEmpty(lastName)
                    || string.IsNullOrEmpty(city)
                    || string.IsNullOrEmpty(email)
                    || string.IsNullOrEmpty(password)
                    || string.IsNullOrEmpty(values))
                {
                    throw new EmptyUserInformationException();
                }

                // Найдет такого пользователя по email.
                var user = await FindUserByEmailOrPhoneNumberAsync(email);

                if (user != null)
                {
                    // Выберет хэш пароля.
                    var passwordHash = await GetUserHashPassword(email);

                    if (passwordHash == null)
                    {
                        passwordHash = await _commonService.HashPasswordAsync(password);

                        // Изменит пароль в таблице пользователей.
                        var getUser = await (from u in _postgreDbContext.Users
                                             where u.Id.Equals(user.UserId)
                                             select u)
                            .FirstOrDefaultAsync();

                        getUser.UserPassword = password;
                        getUser.PasswordHash = passwordHash;

                        await _postgreDbContext.SaveChangesAsync();
                    }

                    // Проверит, есть ли такие данные в таблицы доп.информации пользователей.
                    var checkUserInfoData = await (from ui in _postgreDbContext.UsersInformation
                                                   where ui.UserId.Equals(user.UserId)
                                                   select ui)
                        .FirstOrDefaultAsync();

                    // Если таких данных еще не было, то добавит.
                    if (checkUserInfoData == null)
                    {
                        // Запишет доп.информацию.   
                        await _postgreDbContext.UsersInformation.AddAsync(new UserInformationEntity
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            City = city,
                            Email = email,
                            Password = passwordHash,
                            Values = values,
                            PhoneNumber = user.PhoneNumber,
                            UserId = user.UserId
                        });

                        await _postgreDbContext.SaveChangesAsync();

                        user.IsWriteQuestion = true;

                        await _postgreDbContext.SaveChangesAsync();

                        // Генерит guid код для подтверждения почты.
                        var guid = Guid.NewGuid();

                        // Отправит подтверждение на почту.
                        await _mailingService.SendAcceptEmailAsync(email, $"Подтвердите регистрацию, перейдя по ссылке: <a href='https://gobizy.ru?code={guid}'>Подтвердить</a>", "Gobizy: Подтверждение почты");
                        //http://localhost:4200

                        // Запишет guid в таблицу пользователей.
                        var getUser = await (from u in _postgreDbContext.Users
                                             where u.Id.Equals(user.UserId)
                                             select u)
                            .FirstOrDefaultAsync();

                        getUser.ConfirmEmailCode = guid.ToString();

                        await _postgreDbContext.SaveChangesAsync();
                    }

                    // Иначе обновит.
                    else
                    {
                        checkUserInfoData.FirstName = firstName;
                        checkUserInfoData.LastName = lastName;
                        checkUserInfoData.City = city;
                        checkUserInfoData.Email = email;
                        checkUserInfoData.Password = passwordHash;
                        checkUserInfoData.Values = values;
                        checkUserInfoData.PhoneNumber = user.PhoneNumber;
                        checkUserInfoData.UserId = user.UserId;

                        await _postgreDbContext.SaveChangesAsync();

                        user.IsWriteQuestion = true;

                        await _postgreDbContext.SaveChangesAsync();
                    }

                    result = new UserInformationOutput
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        City = city,
                        Email = email,
                        Values = values,
                        PhoneNumber = user.PhoneNumber
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
        /// Метод найдет пользователя по email или номеру телефона.
        /// </summary>
        /// <param name="data">Email или номер телефона.</param>
        /// <returns>Данные пользователя.</returns>
        public async Task<UserOutput> FindUserByEmailOrPhoneNumberAsync(string data)
        {
            try
            {
                var result = await (from u in _postgreDbContext.Users
                                    where u.Email.Equals(data) || u.PhoneNumber.Equals(data)
                                    select new UserOutput
                                    {
                                        Email = u.Email,
                                        PhoneNumber = u.PhoneNumber,
                                        DateRegister = u.DateRegister,
                                        FirstName = u.FirstName,
                                        LastName = u.LastName,
                                        City = u.City,
                                        IsWriteQuestion = u.IsWriteQuestion,
                                        UserId = u.Id
                                    })
                    .FirstOrDefaultAsync();

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
        /// Метод найдет захэшированный пароль пользователя по логину или email или номеру телефона.
        /// </summary>
        /// <param name="data">Логин или email пользователя.</param>
        /// <returns>Захэшированный пароль.</returns>
        public async Task<string> GetUserHashPassword(string data)
        {
            try
            {
                var result = await (from u in _postgreDbContext.Users
                                    where u.Email.Equals(data) 
                                          || u.PhoneNumber.Equals(data)
                                          || u.UserName.Equals(data)
                                    select u.PasswordHash)
                    .FirstOrDefaultAsync();

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
                var userCode = await (from u in _postgreDbContext.Users
                                      where u.ConfirmEmailCode.Equals(code)
                                      select u)
                    .FirstOrDefaultAsync();

                return userCode != null;
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
                var result = new SuggestionOutput();

                // Если нужно получить одно предложение.
                if (isSingle && !isAll)
                {
                    var getSuggestion = await (from s in _postgreDbContext.Suggestions
                                               where s.IsSingle.Equals(true)
                                                     && s.IsAll.Equals(false)
                                               select new SuggestionOutput
                                               {
                                                   Button1Text = s.Button1Text,
                                                   Button2Text = s.Button2Text,
                                                   IsAll = s.IsAll,
                                                   IsDisplay = s.IsDisplay,
                                                   IsSingle = s.IsSingle,
                                                   Text = s.Text,
                                                   UserId = s.UserId
                                               })
                        .FirstOrDefaultAsync();

                    result = getSuggestion;
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
        /// Метод получит все предложения с флагом IsAll.
        /// </summary>
        /// <param name="isSingle">Получить одно предложение.</param>
        /// <param name="isAll">Получить все предложения.</param>
        /// <returns>Список предложений.</returns>
        public async Task<IEnumerable<SuggestionOutput>> GetAllSuggestionsAsync(bool isSingle, bool isAll)
        {
            try
            {
                IEnumerable<SuggestionOutput> result = null;

                // Если нужно получить список предложений.
                if (!isSingle && isAll)
                {
                    result = await (from s in _postgreDbContext.Suggestions
                                    where s.IsSingle.Equals(false)
                                          && s.IsAll.Equals(true)
                                    select new SuggestionOutput
                                    {
                                        Button1Text = s.Button1Text,
                                        Button2Text = s.Button2Text,
                                        IsAll = s.IsAll,
                                        IsDisplay = s.IsDisplay,
                                        IsSingle = s.IsSingle,
                                        Text = s.Text,
                                        UserId = s.UserId
                                    })
                        .ToListAsync();
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
                var result = await (from b in _postgreDbContext.Breadcrumbs
                                    where b.SelectorPage.Equals(selectorPage)
                                    orderby b.Position
                                    select new BreadcrumbOutput
                                    {
                                        Label = b.Label,
                                        SelectorPage = b.SelectorPage,
                                        Url = b.Url,
                                        Position = b.Position
                                    })
                    .ToListAsync();

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
                var result = await (from u in _postgreDbContext.Users
                                where u.Code.Equals(data)
                                select u.Id)
                    .FirstOrDefaultAsync();

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
        /// <returns>Флаг записи перехода.</returns>
        public async Task<bool> SetTransitionAsync(string account, string transitionType, long referenceId)
        {
            try
            {
                if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(transitionType) || referenceId <= 0)
                {
                    return false;
                }

                var userId = string.Empty;

                var findUser = await FindUserByEmailOrPhoneNumberAsync(account);

                // Если такого пользователя не найдено, значит поищет по коду.
                if (findUser == null)
                {
                    var findUserIdByCode = await FindUserByCodeAsync(account);

                    if (!string.IsNullOrEmpty(findUserIdByCode))
                    {
                        userId = findUserIdByCode;
                    }
                }

                else
                {
                    userId = findUser.UserId;
                }

                // Если никак не найдено.
                if (findUser == null && string.IsNullOrEmpty(userId))
                {
                    return false;
                }

                // Если переход франшизы.
                if (transitionType.Equals("Franchise"))
                {
                    // Проверит существование такой франшизы.
                    var findFranchise = await _postgreDbContext.Franchises
                        .Where(f => f.FranchiseId == referenceId)
                        .FirstOrDefaultAsync();

                    if (findFranchise == null)
                    {
                        return false;
                    }
                }

                // TODO: вернуться когда будет заведена таблица готового бизнеса.
                // Если переход готового бизнеса.
                //else if (expr)
                //{

                //}

                // Проверит, есть ли уже переход у пользователя.
                var findTransition = await _postgreDbContext.Transitions
                    .Where(t => t.UserId.Equals(userId))
                    .FirstOrDefaultAsync();

                // Если перехода нет, то добавит.
                if (findTransition == null)
                {
                    await _postgreDbContext.Transitions.AddAsync(new TransitionEntity
                    {
                        UserId = userId,
                        TransitionType = transitionType,
                        ReferenceId = referenceId
                    });
                }

                // Если есть, то перезапишет его.
                else
                {
                    var updateTransition = new TransitionEntity
                    {
                        UserId = userId,
                        TransitionType = transitionType,
                        ReferenceId = referenceId
                    };

                    _postgreDbContext.Update(updateTransition);
                }

                await _postgreDbContext.SaveChangesAsync();

                return true;
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
                var findUser = await FindUserByEmailOrPhoneNumberAsync(account);

                var userId = string.Empty;

                // Если такого пользователя не найдено, значит поищет по коду.
                if (findUser == null)
                {
                    var findUserIdByCode = await FindUserByCodeAsync(account);

                    if (!string.IsNullOrEmpty(findUserIdByCode))
                    {
                        userId = findUserIdByCode;
                    }
                }

                else
                {
                    userId = findUser.UserId;
                }

                // Если никак не найдено.
                if (findUser == null && string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                var result = await _postgreDbContext.Transitions
                    .Where(t => t.UserId.Equals(userId))
                    .Select(t => new TransitionOutput
                    {
                        ReferenceId = t.ReferenceId,
                        TransitionType = t.TransitionType,
                        UserId = t.UserId
                    })
                    .FirstOrDefaultAsync();

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
    }
}
