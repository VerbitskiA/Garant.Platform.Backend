using System;
using System.Net;
using System.Threading.Tasks;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Mailings.Abstraction;
using MailKit.Net.Smtp;
using MimeKit;

namespace Garant.Platform.Mailings.Service.Sms
{
    /// <summary>
    /// Сервис рассылок.
    /// </summary>
    public class MailingService : IMailingService
    {
        private readonly PostgreDbContext _postgreDbContext;

        public MailingService(PostgreDbContext postgreDbContext)
        {
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод рассылки смс кода подтверждения.
        /// </summary>
        /// <param name="number">Номер телефона, на который будет отправлено смс.</param>
        /// <param name="code">Код подтверждения.</param>
        public async Task SendAcceptCodeSmsAsync(string number, string code)
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

        /// <summary>
        /// Метод отправит код подтверждения на почту.
        /// </summary>
        /// <param name="code">Код подтверждения.</param>
        /// <param name="emailTo">email на который отправить сообщение.</param>
        public async Task SendAcceptCodeMailAsync(string code, string emailTo)
        {
            try
            {
                string host = null;
                int port = 0;

                // Для mail.ru или bk.ru.
                if (emailTo.Contains("@mail") || emailTo.Contains("@bk"))
                {
                    host = "smtp.mail.ru";
                    port = 2525;
                }

                // Для gmail.com.
                else if (emailTo.Contains("@gmail"))
                {
                    host = "smtp.gmail.com";
                    port = 587;
                }

                // Для yandex.ru.
                else if (emailTo.Contains("@yandex"))
                {
                    host = "smtp.yandex.ru";
                    port = 465;
                }

                var emailMessage = new MimeMessage();
                //emailMessage.From.Add(new MailboxAddress("gobizy@bk.ru"));
                emailMessage.From.Add(new MailboxAddress("dead.toni@mail.ru"));
                emailMessage.To.Add(new MailboxAddress(emailTo));
                emailMessage.Subject = "Gobizy: Код подтверждения";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "Код подтверждения: " + code
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
                //await client.AuthenticateAsync("gobizy@bk.ru", "Garant1234");//ZUrJBp0GXD3xekTdU9YZ
                await client.AuthenticateAsync("dead.toni@mail.ru", "13467kvm");//ZUrJBp0GXD3xekTdU9YZ
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
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
