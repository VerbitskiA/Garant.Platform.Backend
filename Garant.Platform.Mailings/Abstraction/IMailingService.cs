using System.Threading.Tasks;

namespace Garant.Platform.Mailings.Abstraction
{
    /// <summary>
    /// Абстракция сервиса рассылок.
    /// </summary>
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

        /// <summary>
        /// Метод отправит подтверждение на почту.
        /// </summary>
        /// <param name="mailTo">Адрес кому отправить.</param>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <param name="messageTitle">Заголовок сообщения.</param>
        Task SendAcceptEmailAsync(string mailTo, string messageBody, string messageTitle);

        /// <summary>
        /// Метод отправит на почту администрации сервиса оповещение о созданной карточке.
        /// </summary>
        /// <param name="cardType">Тип карточки.</param>
        /// <param name="cardUrl">Ссылка на карточку.</param>
        Task SendMailAfterCreateCardAsync(string cardType, string cardUrl = null);
    }
}
