using System;

namespace Garant.Platform.Models.Blog.Input
{
    /// <summary>
    /// Класс входной модели.
    /// </summary>
    public class IncrementViewNew
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Идентификатор новости.
        /// </summary>
        public long NewId { get; set; }
    }
}
