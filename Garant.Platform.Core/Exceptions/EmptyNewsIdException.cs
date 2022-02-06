using System;

namespace Garant.Platform.Core.Exceptions
{
    public class EmptyNewsIdException : Exception
    {
        public EmptyNewsIdException() : base("Id новости был <= 0")
        {
        }
    }
}