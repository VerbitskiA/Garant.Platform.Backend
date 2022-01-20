namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если при подтверждении акта передан не тот тип документа.
    /// </summary>
    public class ErrorApproveDocumentTypeException : UserMessageException
    {
        public ErrorApproveDocumentTypeException(string message) : base(message)
        {
        }
    }
}
