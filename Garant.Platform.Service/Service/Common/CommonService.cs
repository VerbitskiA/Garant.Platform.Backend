using System;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Mailings.Abstraction;
using Garant.Platform.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Service.Service.Common
{
    /// <summary>
    /// Сервис общих методов.
    /// </summary>
    public sealed class CommonService : ICommonService
    {
        private readonly IMailingSmsService _mailigSmsService;
        private readonly PostgreDbContext _postgreDbContext;

        public CommonService(PostgreDbContext postgreDbContext, IMailingSmsService mailigSmsService)
        {
            _postgreDbContext = postgreDbContext;
            _mailigSmsService = mailigSmsService;
        }

        /// <summary>
        /// Метод создает код. Кол-во цифр зависит от переданного типа.
        /// </summary>
        /// <param name="number">Номер телефона.</param>
        /// <param name="type">Тип кода. От этого зависит алгоритм создания кода и кол-во цифр.</param>
        /// <returns>Флаг успеха.</returns>
        public async Task GenerateAcceptCodeAsync(string number, string type)
        {
            var random = new Random();

            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    throw new EmptyTypeMailingException();
                }

                if (type.Equals("sms"))
                {
                    // Создаст код подтверждения из 5 цифр.
                    var code = random.Next(10000, 99999).ToString("D4");

                    // Запишет код подтверждения в базу или обновит его.
                    await SaveCodeAsync(number, code);

                    // Отправит код подтверждения по смс.
                    await _mailigSmsService.SendMailAcceptCodeSmsAsync(number, code);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Метод запишет код подтвержедния в БД.
        /// </summary>
        /// <param name="number">Номер телефона.</param>
        /// <param name="code">Код подтверждения.</param>
        private async Task SaveCodeAsync(string number, string code)
        {
            try
            {
                // Если найдет такой номер телеофна в БД, то перезпишет код.
                var findNumber = await (from u in _postgreDbContext.Users
                                        where u.PhoneNumber.Equals(number)
                                        select u)
                    .FirstOrDefaultAsync();

                // Если такой номер был, то обновит код пользователю.
                if (findNumber != null)
                {
                    findNumber.Code = code;
                    await _postgreDbContext.SaveChangesAsync();

                    return;
                }

                // Такого номера не было, добавит пользователя.
                await _postgreDbContext.Users.AddAsync(new UserEntity
                {
                    PhoneNumber = number,
                    Code = code,
                    DateRegister = DateTime.Now,
                    RememberMe = false,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                });
                await _postgreDbContext.SaveChangesAsync();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
