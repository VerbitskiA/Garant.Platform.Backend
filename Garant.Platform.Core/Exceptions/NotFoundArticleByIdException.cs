using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если не было найдено статьи по ее Id.
    /// </summary>
    public class NotFoundArticleByIdException : Exception
    {
        public NotFoundArticleByIdException(long articleId) : base($"Статьи с ArticleId {articleId} не найдено")
        {
        }
    }
}