using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если не было найдено такой новости.
    /// </summary>
    public class NotFoundNewException : Exception
    {
        public NotFoundNewException(long newsId) : base($"Новости с {nameof(newsId)}: {newsId} не найдено!")
        {

        }
    }
}
