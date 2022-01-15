using Garant.Platform.Core.Exceptions;

namespace Garant.Platform.Commerce.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если заказ не был найден.
    /// </summary>
    public class ErrorFindOrderException : UserMessageException
    {
        public ErrorFindOrderException(string message) : base(message)
        {
        }
    }
}
