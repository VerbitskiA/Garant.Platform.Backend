using System.Threading.Tasks;

namespace Garant.Platform.Mailings.Abstraction
{
    public interface IMailingService
    {
        /// <summary>
        /// Метод рассылки смс кода подтверждения.
        /// </summary>
        /// <param name="number">Номер тлефона, на который будет отправлено смс.</param>
        /// <param name="code">Код подтверждения.</param>
        Task SendAcceptCodeSmsAsync(string number, string code);

        /// <summary>
        /// Метод отправит код подтверждения на почту.
        /// </summary>
        /// <param name="code">Код подтверждения.</param>
        /// <param name="emailTo">email на который отправить сообщение.</param>
        Task SendAcceptCodeMailAsync(string code, string emailTo);
    }
}
