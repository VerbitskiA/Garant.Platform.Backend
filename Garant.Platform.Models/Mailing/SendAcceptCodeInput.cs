namespace Garant.Platform.Models.Mailing
{
    /// <summary>
    /// Класс входной модели отправки кода подтверждения по смс.
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
    }
}
