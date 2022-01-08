using Garant.Platform.Core.Exceptions;

namespace Garant.Platform.Commerce.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если передан некорректный Id платежа.
    /// </summary>
    public class EmptyOrderIdException : UserMessageException
    {
        public EmptyOrderIdException(string message) : base(message)
        {
        }
    }
}
