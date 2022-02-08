using System;
using System.Net;
using System.Threading.Tasks;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Mailings.Abstraction;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Garant.Platform.Mailings.Service
{
    /// <summary>
    /// Сервис рассылок.
    /// </summary>
    public class MailingService : IMailingService
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IConfiguration _configuration;

        public MailingService(PostgreDbContext postgreDbContext, IConfiguration configuration)
        {
            _postgreDbContext = postgreDbContext;
            _configuration = configuration;
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
                var data = await GetHostAndPortAsync(emailTo);
                var email = _configuration.GetSection("MailingSettings:Email").Value;
                var password = _configuration.GetSection("MailingSettings:Password").Value;

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(email));
                emailMessage.To.Add(new MailboxAddress(emailTo));
                emailMessage.Subject = "Gobizy: Код подтверждения";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "Код подтверждения: " + code
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(data.Item1, data.Item2, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(email, password);
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

        private async Task<(string, int)> GetHostAndPortAsync(string param)
        {
            // Для mail.ru или bk.ru.
            if (param.Contains("@mail") || param.Contains("@bk") || param.Contains("@list"))
            {
                return await Task.FromResult(("smtp.mail.ru", 2525));
            }

            // Для gmail.com.
            else if (param.Contains("@gmail"))
            {
                return await Task.FromResult(("smtp.gmail.com", 587));
            }

            // Для yandex.ru.
            else if (param.Contains("@yandex"))
            {
                return await Task.FromResult(("smtp.yandex.ru", 465));
            }

            return (null, 0);
        }

        /// <summary>
        /// Метод отправит подтверждение на почту.
        /// </summary>
        /// <param name="mailTo">Адрес кому отправить.</param>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <param name="messageTitle">Заголовок сообщения.</param>
        public async Task SendAcceptEmailAsync(string mailTo, string messageBody, string messageTitle)
        {
            try
            {
                var data = await GetHostAndPortAsync(mailTo);
                var email = _configuration.GetSection("MailingSettings:Email").Value;
                var password = _configuration.GetSection("MailingSettings:Password").Value;

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(email));
                emailMessage.To.Add(new MailboxAddress(mailTo));
                emailMessage.Subject = messageTitle;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = messageBody
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(data.Item1, data.Item2, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(email, password);
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
