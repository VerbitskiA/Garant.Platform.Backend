namespace Garant.Platform.Models.Mailing.Input
{
    /// <summary>
    /// Класс входной модели отправки кода подтверждения.
    /// </summary>
    public class SendAcceptCodeInput
    {
        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Код подтверждения.
        /// </summary>
        public string Code { get; set; }
    }
}
