using System;
using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Mailings.Abstraction;

namespace Garant.Platform.Service.Service.Common
{
    /// <summary>
    /// Сервис общих методов.
    /// </summary>
    public sealed class CommonService : ICommonService
    {
        private readonly IMailingSmsService _mailigSmsService;

        public CommonService()
        {
           
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
                //var scope = AutoFac.Resolve<IUserService>();
                //Console.WriteLine();
                if (string.IsNullOrEmpty(type))
                {
                    throw new EmptyTypeMailingException();
                }

                if (type.Equals("sms"))
                {
                    // Создаст код подтверждения из 5 цифр.
                    var code = random.Next(10000, 99999).ToString("D4");

                    // TODO: сюда добавить запись кода в базу.
                    // Запишет код подтверждения в базу.

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
    }
}
