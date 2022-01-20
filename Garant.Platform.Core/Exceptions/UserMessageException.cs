using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Базовый класс для исключений, текст сообщения которых нужно показать пользователю.
    ///  </summary>
    public class UserMessageException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Exception"></see> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public UserMessageException(string message) : base(message) { }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception"></see> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public UserMessageException(string message, Exception innerException) : base(message, innerException) { }
    }
}
