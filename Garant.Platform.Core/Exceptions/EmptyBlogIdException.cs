using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если BlogId <= 0.
    /// </summary>
    public class EmptyBlogIdException : Exception
    {
        public EmptyBlogIdException() : base("BlogId был <= 0")
        {
        }
    }
}