using Garant.Platform.Core.Exceptions;

namespace Garant.Platform.Base.Exceptions
{
    /// <summary>
    /// Исключение возникает, если сумма была некорректной.
    /// </summary>
    public class EmptySumException : UserMessageException
    {
        public EmptySumException(string message) : base(message) {}
    }
}
