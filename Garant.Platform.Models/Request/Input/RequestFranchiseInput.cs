using System;

namespace Garant.Platform.Models.Request.Input
{
    /// <summary>
    /// Класс входной модели создания заявки франшизы.
    /// </summary>
    public class RequestFranchiseInput
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
        /// Город.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Id франшизы, по которой оставлена заявка.
        /// </summary>
        public long FranchiseId { get; set; }
    }
}
