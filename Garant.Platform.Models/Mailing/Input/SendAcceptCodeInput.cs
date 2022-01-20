namespace Garant.Platform.Models.Mailing.Input
{
    /// <summary>
    /// Класс входной модели отправки кода подтверждения.
    /// </summary>
    public class SendAcceptCodeInput
    {
        /// <summary>
        /// Код подтверждения.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Номер телефона, на который отправит код подтверждения.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Данные, содержащие телефон или email.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Тип отправки (sms или email).
        /// </summary>
        public string Type { get; set; }
    }
}
