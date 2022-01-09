using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.Models.Entities.Transition;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.Footer.Output;
using Garant.Platform.Models.Header.Output;
using Garant.Platform.Models.Suggestion.Output;
using Garant.Platform.Models.Transition.Output;
using Garant.Platform.Models.User.Input;
using Garant.Platform.Models.User.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Services.Service.User
{
    /// <summary>
    /// Репозиторий пользователей для работы с БД.
    /// </summary>
    public sealed class UserRepository : IUserRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly ICommonService _commonService;

        public UserRepository(PostgreDbContext postgreDbContext, ICommonService commonService)
        {
            _postgreDbContext = postgreDbContext;
            _commonService = commonService;
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
                                    where u.Email.Equals(data)
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

                if (result == null)
                {
                    result = await (from u in _postgreDbContext.Users
                            where u.PhoneNumber.Equals(data)
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
        /// <param name="kpp">КПП.</param>
        /// <param name="bik">БИК.</param>
        /// <param name="defaultBankName">Название банка которое нужно сохранить по умолчанию.</param>
        /// <returns>Данные пользователя.</returns>
        public async Task<UserInformationOutput> SaveUserInfoAsync(string firstName, string lastName, string city, string email, string password, string values, string guid, int kpp, int bik, string defaultBankName)
        {
            try
            {
                var result = new UserInformationOutput();

                // Найдет такого пользователя по email.
                var user = await FindUserByEmailOrPhoneNumberAsync(email);

                if (user != null)
                {
                    // Выберет хэш пароля.
                    var passwordHash = await GetUserHashPasswordAsync(email);

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
                        var addInfo = new UserInformationEntity
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            City = city,
                            Email = email,
                            Password = passwordHash,
                            Values = values,
                            PhoneNumber = user.PhoneNumber,
                            UserId = user.UserId,
                            Kpp = kpp,
                            Bik = bik
                        };

                        if (!string.IsNullOrEmpty(defaultBankName))
                        {
                            addInfo.DefaultBankName = defaultBankName;
                        }

                        // Запишет доп.информацию.   
                        await _postgreDbContext.UsersInformation.AddAsync(addInfo);

                        await _postgreDbContext.SaveChangesAsync();

                        user.IsWriteQuestion = true;

                        await _postgreDbContext.SaveChangesAsync();

                        // Запишет guid в таблицу пользователей.
                        var getUser = await (from u in _postgreDbContext.Users
                                             where u.Id.Equals(user.UserId)
                                             select u)
                            .FirstOrDefaultAsync();

                        getUser.ConfirmEmailCode = guid;

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
                        checkUserInfoData.Kpp = kpp;
                        checkUserInfoData.Bik = bik;

                        if (!string.IsNullOrEmpty(defaultBankName))
                        {
                            checkUserInfoData.DefaultBankName = defaultBankName;
                        }

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
                        PhoneNumber = user.PhoneNumber,
                        Kpp = kpp,
                        Bik = bik,
                        DefaultBankName = defaultBankName
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
        /// Метод найдет захэшированный пароль пользователя по логину или email или номеру телефона.
        /// </summary>
        /// <param name="data">Логин или email пользователя.</param>
        /// <returns>Захэшированный пароль.</returns>
        public async Task<string> GetUserHashPasswordAsync(string data)
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

                    if (getSuggestion != null)
                    {
                        result = getSuggestion;
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
        /// Метод сформирует хлебные крошки для страницы.
        /// </summary>
        /// <param name="selectorPage"> Селектор страницы, для которой нужно сформировать хлебные крошки.</param>
        /// <returns>Список хлебных крошек.</returns>
        public async Task<List<BreadcrumbOutput>> GetBreadcrumbsAsync(string selectorPage)
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

                // Если переход готового бизнеса.
                else if (transitionType.Equals("Business"))
                {
                    // Проверит существование такого бизнеса.
                    var findBusiness = await _postgreDbContext.Businesses
                        .Where(f => f.BusinessId == referenceId)
                        .FirstOrDefaultAsync();

                    if (findBusiness == null)
                    {
                        return false;
                    }
                }

                // Проверит, есть ли уже переход у пользователя.
                var findTransition = await _postgreDbContext.Transitions
                    .AnyAsync(t => t.UserId.Equals(userId)
                                   && t.ReferenceId == referenceId
                                   && t.TransitionType.Equals(transitionType));

                var transition = new TransitionEntity
                {
                    UserId = userId,
                    TransitionType = transitionType,
                    ReferenceId = referenceId
                };

                // Доп. проверки для чата.
                if (!string.IsNullOrEmpty(otherId) && !string.IsNullOrEmpty(typeItem) || !string.IsNullOrEmpty(typeItem) && typeItem.Equals("PaymentAct"))
                {
                    transition.OtherId = otherId;
                    transition.TypeItem = typeItem;
                }

                // Если перехода нет, то добавит.
                if (!findTransition)
                {
                    await _postgreDbContext.Transitions.AddAsync(transition);
                }

                // Если есть, то перезапишет его.
                else
                {
                    _postgreDbContext.Update(transition);
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

                // Найдет Id последнего перехода у пользователя.
                var maxTransitionId = await _postgreDbContext.Transitions.MaxAsync(t => t.TransitionId);

                var result = await _postgreDbContext.Transitions
                    .Where(t => t.UserId.Equals(userId) && t.TransitionId == maxTransitionId)
                    .Select(t => new TransitionOutput
                    {
                        ReferenceId = t.ReferenceId,
                        TransitionType = t.TransitionType,
                        UserId = t.UserId,
                        OtherId = t.OtherId,
                        TypeItem = t.TypeItem
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

        /// <summary>
        /// Метод получит переход пользователя по параметрам.
        /// </summary>
        /// <param name="referenceId">Id заказа или предмета сделки.</param>
        /// <param name="otherId">Id заказа в системе банка.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Данные перехода.</returns>
        public async Task<TransitionOutput> GetTransitionWithParamsAsync(long referenceId, string transitionType, string userId)
        {
            try
            {
                var result = await _postgreDbContext.Transitions
                    .OrderByDescending(t => t.TransitionId)
                    .Where(t => t.ReferenceId == referenceId
                                && t.TransitionType.Equals(transitionType)
                                && t.UserId.Equals(userId))
                    .Select(t => new TransitionOutput
                    {
                        TransitionType = t.TransitionType,
                        ReferenceId = t.ReferenceId,
                        OtherId = t.OtherId
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

        /// <summary>
        /// Метод получит фио авторизованного пользователя.
        /// </summary>
        /// <param name="account">Аккаунт.</param>
        /// <returns>Данные пользователя.</returns>
        public async Task<UserOutput> GetUserFioAsync(string account)
        {
            try
            {
                UserOutput result = null;

                // Ищет по логину.
                result = await _postgreDbContext.Users
                    .Where(u => u.UserName.Equals(account))
                    .Select(u => new UserOutput
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Patronymic = u.Patronymic,
                        FullName = (u.LastName ?? string.Empty) + " " + (u.FirstName ?? string.Empty) + " " + (u.Patronymic ?? string.Empty)
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    // Ищет по почте.
                    result = await _postgreDbContext.Users
                        .Where(u => u.Email.Equals(account))
                        .Select(u => new UserOutput
                        {
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Patronymic = u.Patronymic,
                            FullName = (u.LastName ?? string.Empty) + (u.FirstName ?? string.Empty) + (u.Patronymic ?? string.Empty)
                        })
                        .FirstOrDefaultAsync();
                }

                if (result == null)
                {
                    // Ищет по коду.
                    result = await _postgreDbContext.Users
                        .Where(u => u.Code.Equals(account))
                        .Select(u => new UserOutput
                        {
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Patronymic = u.Patronymic,
                            FullName = (u.LastName ?? string.Empty) + (u.FirstName ?? string.Empty) + (u.Patronymic ?? string.Empty)
                        })
                        .FirstOrDefaultAsync();
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
        /// Метод универсально найдет Id пользователя.
        /// </summary>
        /// <param name="account">Данные для авторизации.</param>
        /// <returns>Id пользователя.</returns>
        public async Task<string> FindUserIdUniverseAsync(string account)
        {
            try
            {
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

                return userId;
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
                // Найдет пользователя.
                var userId = await FindUserIdUniverseAsync(account);
                UserInformationEntity user = null;

                // Проверит, заполнял ли он данные о себе.
                if (!string.IsNullOrEmpty(userId))
                {
                    user = await _postgreDbContext.UsersInformation
                        .Where(u => u.UserId.Equals(userId))
                        .FirstOrDefaultAsync();
                }

                return user != null;
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
        /// <param name="userInformationInput">Входная модель.</param>
        /// <param name="account">Логин или Email.</param>
        /// <param name="documentName">Название документа.</param>
        /// <returns>Данные формы.</returns>
        public async Task<UserInformationOutput> SaveProfileFormAsync(UserInformationInput userInformationInput, string account, string documentName)
        {
            try
            {
                var userId = await FindUserIdUniverseAsync(account);

                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                // Выберет запись с данными информации о пользователе.
                var userData = await _postgreDbContext.UsersInformation
                    .Where(i => i.UserId.Equals(userId))
                    .FirstOrDefaultAsync();

                if (userData == null)
                {
                    return null;
                }

                // Для главной формы.
                if (userInformationInput.TypeForm.Equals("MainData"))
                {
                    userData.FirstName = userInformationInput.FirstName;
                    userData.LastName = userInformationInput.LastName;
                    userData.Email = userInformationInput.Email;
                    userData.PhoneNumber = userInformationInput.PhoneNumber;
                    userData.City = userInformationInput.City;
                    userData.DateBirth = userInformationInput.DateBirth;
                    userData.Patronymic = userInformationInput.Patronymic;
                }

                // Для формы реквизитов.
                else if (userInformationInput.TypeForm.Equals("Requisites"))
                {
                    userData.Inn = userInformationInput.Inn;
                    userData.Pc = userInformationInput.Pc;
                    userData.PassportSerial = userInformationInput.PassportSerial;
                    userData.PassportNumber = userInformationInput.PassportNumber;
                    userData.DateGive = userInformationInput.DateGive;
                    userData.WhoGive = userInformationInput.WhoGive;
                    userData.Code = userInformationInput.Code;
                    userData.AddressRegister = userInformationInput.AddressRegister;
                    userData.DocumentName = documentName;
                }

                // Обновит данные.
                _postgreDbContext.UsersInformation.Update(userData);
                await _postgreDbContext.SaveChangesAsync();

                var result = new UserInformationOutput
                {
                    Inn = userData.Inn,
                    Pc = userData.Pc,
                    PassportSerial = userData.PassportSerial,
                    PassportNumber = userData.PassportNumber,
                    DateGive = Convert.ToDateTime(userData.DateGive).ToString("yyyy-MM-dd"),
                    WhoGive = userData.WhoGive,
                    Code = userData.Code,
                    AddressRegister = userData.AddressRegister,
                    DocumentName = documentName,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = userData.Email,
                    PhoneNumber = userData.PhoneNumber,
                    City = userData.City,
                    DateBirth = userData.DateBirth.ToString("yyyy-MM-dd"),
                    Patronymic = userData.Patronymic,
                    Password = userData.Password
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
        /// Метод универсально найдет все данные пользователя.
        /// </summary>
        /// <param name="account">Все данные для авторизации.</param>
        /// <returns>Данные пользователя.</returns>
        public async Task<UserEntity> FindUserUniverseAsync(string account)
        {
            try
            {
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

                var result = await _postgreDbContext.Users.Where(u => u.Id.Equals(userId)).FirstOrDefaultAsync();

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
        /// Метод получит информацию профиля пользователя.
        /// </summary>
        /// <param name="account">Логин или email пользователя.</param>
        /// <returns>Данные профиля.</returns>
        public async Task<UserInformationOutput> GetProfileInfoAsync(string account)
        {
            try
            {
                var userId = await FindUserIdUniverseAsync(account);

                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                var profileData = await _postgreDbContext.UsersInformation
                    .Where(i => i.UserId.Equals(userId))
                    .Select(i => new UserInformationOutput
                    {
                        Inn = i.Inn,
                        Pc = i.Pc,
                        PassportSerial = i.PassportSerial,
                        PassportNumber = i.PassportNumber,
                        DateGive = Convert.ToDateTime(i.DateGive).ToString("yyyy-MM-dd"),
                        WhoGive = i.WhoGive,
                        Code = i.Code,
                        AddressRegister = i.AddressRegister,
                        DocumentName = i.DocumentName,
                        FirstName = i.FirstName,
                        LastName = i.LastName,
                        Email = i.Email,
                        PhoneNumber = i.PhoneNumber,
                        City = i.City,
                        DateBirth = i.DateBirth.ToString("yyyy-MM-dd"),
                        Patronymic = i.Patronymic,
                        Values = i.Values,
                        Kpp = i.Kpp,
                        Bik = i.Bik
                    })
                    .FirstOrDefaultAsync();

                return profileData;
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
                var result = await _postgreDbContext.ProfileNavigations
                    .Select(n => new ProfileNavigationOutput
                    {
                        IsHide = n.IsHide,
                        NavigationLink = n.NavigationLink,
                        NavigationText = n.NavigationText,
                        Position = n.Position
                    })
                    .OrderBy(o => o.Position)
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
        /// Метод получит информацию профиля пользователя по его Id.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Данные профиля.</returns>
        public async Task<UserInformationOutput> GetUserProfileInfoByIdAsync(string userId)
        {
            try
            {
                var profileData = await _postgreDbContext.UsersInformation
                    .Where(i => i.UserId.Equals(userId))
                    .Select(i => new UserInformationOutput
                    {
                        Inn = i.Inn,
                        Pc = i.Pc,
                        PassportSerial = i.PassportSerial,
                        PassportNumber = i.PassportNumber,
                        DateGive = Convert.ToDateTime(i.DateGive).ToString("yyyy-MM-dd"),
                        WhoGive = i.WhoGive,
                        Code = i.Code,
                        AddressRegister = i.AddressRegister,
                        DocumentName = i.DocumentName,
                        FirstName = i.FirstName,
                        LastName = i.LastName,
                        Email = i.Email,
                        PhoneNumber = i.PhoneNumber,
                        City = i.City,
                        DateBirth = i.DateBirth.ToString("yyyy-MM-dd"),
                        Patronymic = i.Patronymic,
                        Values = i.Values
                    })
                    .FirstOrDefaultAsync();

                return profileData;
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
        /// Проверит, подтверждал ли пользователь свою почту, если нет, то попросит подтвердить.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Статус проверки.</returns>
        public async Task<bool> CheckConfirmEmailAsync(string userId)
        {
            try
            {
                var result = await _postgreDbContext.Users
                    .Where(u => u.Id.Equals(userId))
                    .Select(u => u.EmailConfirmed)
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
