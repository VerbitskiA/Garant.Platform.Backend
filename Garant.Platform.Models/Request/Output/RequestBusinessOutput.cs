using System;

namespace Garant.Platform.Models.Request.Output
{
    /// <summary>
    /// Класс выходной модели создания заявки бизнеса.
    /// </summary>
    public class RequestBusinessOutput
    {
        /// <summary>
        /// PK.
        /// </summary>
        public long RequestId { get; set; }

        /// <summary>
        /// Id пользователя, который оставил заявку.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Дата создания заявки.
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Id франшизы, по которой оставлена заявка.
        /// </summary>
        public long FranchiseId { get; set; }

        /// <summary>
        /// Флаг успешно ли добавление.
        /// </summary>
        public bool IsSuccessCreatedRequest { get; set; }

        /// <summary>
        /// Текст после добавления заявки.
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// Id бизнеса, по которому оставлена заявка.
        /// </summary>
        public long BusinessId { get; set; }
    }
}
