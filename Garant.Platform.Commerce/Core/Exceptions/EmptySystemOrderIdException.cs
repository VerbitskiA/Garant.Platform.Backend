using Garant.Platform.Core.Exceptions;

namespace Garant.Platform.Commerce.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если передан некорректный Id платежа в системе банка.
    /// </summary>
    public class EmptySystemOrderIdException : UserMessageException
    {
        public EmptySystemOrderIdException(string message) : base(message)
        {
        }
    }
}
