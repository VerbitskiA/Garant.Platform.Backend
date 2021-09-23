using System;
using System.Net;
using System.Threading.Tasks;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Mailings.Abstraction;

namespace Garant.Platform.Mailings.Service.Sms
{
    public class MailingSmsService : IMailingSmsService
    {
        public MailingSmsService()
        {
            
        }

        /// <summary>
        /// Метод рассылки смс кода подтверждения.
        /// </summary>
        /// <param name="number">Номер тлефона, на который будет отправлено смс.</param>
        /// <param name="code">Код подтверждения.</param>
        public async Task SendMailAcceptCodeSmsAsync(string number, string code)
        {
            try
            {
                if (string.IsNullOrEmpty(number))
                {
                    throw new EmptyPhoneNumberException();
                }

                // Создаст запрос.
                var request = WebRequest.Create($"https://smsc.ru/sys/send.php?login=sierra&psw=9V@hu3.cfAi_ec.&phones={number}&mes=Код подтверждения {code}");

                // Отправит запрос.
                await request.GetResponseAsync();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
