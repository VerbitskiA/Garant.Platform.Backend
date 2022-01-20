namespace Garant.Platform.Models.Mailing.Output
{
    /// <summary>
    /// Класс выходной модели рассылок.
    /// </summary>
    public class MailngOutput
    {
        /// <summary>
        /// Флаг успеха рассылки.
        /// </summary>
        public bool IsSuccessMailing { get; set; }

        /// <summary>
        /// Тип рассылки. Это нужно, чтобы отобразить текст "Мы отправили код подтверждения вам на почту" или "Мы отправили код подтверждения на ваш телефон".
        /// </summary>
        public string TypeMailing { get; set; }
    }
}
