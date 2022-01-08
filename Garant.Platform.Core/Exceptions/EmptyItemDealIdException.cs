using System;

namespace Garant.Platform.Core.Exceptions
{
    public class EmptyItemDealIdException : Exception
    {
        public EmptyItemDealIdException() : base("Не передан Id предмета сделки.") { }
    }
}
