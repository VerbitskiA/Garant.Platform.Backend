namespace Garant.Platform.Models.Request.Input
{
    /// <summary>
    /// Класс входной модели создания заявки бизнеса.
    /// </summary>
    public class RequestBusinessInput
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Id бизнеса, по которому оставлена заявка.
        /// </summary>
        public long BusinessId { get; set; }
    }
}
