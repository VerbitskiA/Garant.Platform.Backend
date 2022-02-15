using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если не бвло найдено блога с таким BlogId.
    /// </summary>
    public class NotFoundBlogException : Exception
    {
        public NotFoundBlogException(long blogId) : base($"Блога с BlogId: {blogId} не найдено")
        {
            
        }
    }
}