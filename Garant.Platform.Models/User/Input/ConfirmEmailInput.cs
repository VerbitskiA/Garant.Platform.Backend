namespace Garant.Platform.Models.User.Input
{
    /// <summary>
    /// Класс входной модели подтверждения почты.
    /// </summary>
    public class ConfirmEmailInput
    {
        /// <summary>
        /// Временный код подтверждения (guid).
        /// </summary>
        public string Code { get; set; }
    }
}
