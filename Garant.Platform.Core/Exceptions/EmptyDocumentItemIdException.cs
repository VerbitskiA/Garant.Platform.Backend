namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если не передан Id документа сделки.
    /// </summary>
    public class EmptyDocumentItemIdException : UserMessageException
    {
        public EmptyDocumentItemIdException() : base("Не передан Id документа предмета сделки.")
        {

        }
    }
}
