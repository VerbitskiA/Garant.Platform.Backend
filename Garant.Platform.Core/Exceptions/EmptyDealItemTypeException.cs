namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Искючение возникает, если не передан тип предмета сделки.
    /// </summary>
    public class EmptyDealItemTypeException : UserMessageException
    {
        public EmptyDealItemTypeException() :base("Не передан тип предмета сделки.") {}
    }
}
