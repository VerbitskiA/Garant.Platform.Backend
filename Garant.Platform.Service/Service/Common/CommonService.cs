using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Mailings.Abstraction;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.Mailing.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Service.Service.Common
{
    /// <summary>
    /// Сервис общих методов.
    /// </summary>
    public sealed class CommonService : ICommonService
    {
        private readonly IMailingService _mailigSmsService;
        private readonly PostgreDbContext _postgreDbContext;

        public CommonService(PostgreDbContext postgreDbContext, IMailingService mailigSmsService)
        {
            _postgreDbContext = postgreDbContext;
            _mailigSmsService = mailigSmsService;
        }

        /// <summary>
        /// Метод создает код. Кол-во цифр зависит от переданного типа.
        /// </summary>
        /// <param name="data">Телефон или почта.</param>
        /// <returns>Флаг успеха.</returns>
        public async Task<MailngOutput> GenerateAcceptCodeAsync(string data)
        {
            await using var transaction = await _postgreDbContext.Database.BeginTransactionAsync();

            try
            {
                var random = new Random();
                string type = null;

                if (string.IsNullOrEmpty(data))
                {
                    throw new EmptyTypeMailingException();
                }

                // Создаст код подтверждения из 5 цифр.
                var code = random.Next(10000, 99999).ToString("D4");

                // Если передали почту.
                if (data.Contains("@"))
                {
                    type = "mail";
                }

                // Если не почту, тогда по sms.
                else if (!data.Contains("@"))
                {
                    type = "sms";
                }

                if (type.Equals("sms"))
                {
                    // Запишет код подтверждения в базу или обновит его.
                    await SaveCodeAsync(data, code);

                    // Отправит код подтверждения по смс.
                    await _mailigSmsService.SendAcceptCodeSmsAsync(data, code);
                    await transaction.CommitAsync();
                }

                else if (type.Equals("mail"))
                {
                    // Запишет код подтверждения в базу или обновит его.
                    await SaveCodeAsync(data, code);

                    await _mailigSmsService.SendAcceptCodeMailAsync(code, data);
                    await transaction.CommitAsync();
                }

                var result = new MailngOutput
                {
                    IsSuccessMailing = true,
                    TypeMailing = type
                };

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);

                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();

                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Метод запишет код подтвержедния в БД.
        /// </summary>
        /// <param name="number">Данные.</param>
        /// <param name="code">Код подтверждения.</param>
        private async Task SaveCodeAsync(string data, string code)
        {
            try
            {
                UserEntity findNumber = null;
                var isEmail = data.Contains("@");

                if (!isEmail)
                {
                    // Если найдет такой номер телеофна в БД, то перезапишет код.
                    findNumber = await (from u in _postgreDbContext.Users
                                        where u.PhoneNumber.Equals(data)
                                        select u)
                        .FirstOrDefaultAsync();
                }

                else
                {
                    // Если найдет такой username в БД, то перезапишет код.
                    findNumber = await (from u in _postgreDbContext.Users
                                        where u.UserName.Equals(data)
                                        select u)
                        .FirstOrDefaultAsync();
                }

                // Если такой номер был, то обновит код пользователю.
                if (findNumber != null)
                {
                    findNumber.Code = code;
                    await _postgreDbContext.SaveChangesAsync();

                    return;
                }

                var insertData = new UserEntity
                {
                    Code = code,
                    DateRegister = DateTime.Now,
                    RememberMe = false,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                };

                if (isEmail)
                {
                    insertData.UserName = data;
                    insertData.Email = data;
                }

                else
                {
                    insertData.PhoneNumber = data;
                }

                // Такого номера не было, добавит пользователя.
                await _postgreDbContext.Users.AddAsync(insertData);
                await _postgreDbContext.SaveChangesAsync();
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
