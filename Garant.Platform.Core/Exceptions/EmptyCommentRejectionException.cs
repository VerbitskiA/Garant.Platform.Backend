using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет, если не заполнен комментарий отклонения публикации карточки.
    /// </summary>
    public class EmptyCommentRejectionException : Exception
    {
        public EmptyCommentRejectionException(): base("Не заполнен комментарий отклонения публикации карточки")
        {
        }
    }
}