using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если почта некорректная.
    /// </summary>
    public class EmailNotCorrectFormatException : UserMessageException
    {
        public EmailNotCorrectFormatException(string message) : base(message)
        {
        }

        public EmailNotCorrectFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}