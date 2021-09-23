using System.Threading.Tasks;

namespace Garant.Platform.Mailings.Abstraction
{
    public interface IMailingSmsService
    {
        /// <summary>
        /// Метод рассылки смс кода подтверждения.
        /// </summary>
        /// <param name="number">Номер тлефона, на который будет отправлено смс.</param>
        /// <param name="code">Код подтверждения.</param>
        Task SendMailAcceptCodeSmsAsync(string number, string code);
    }
}
