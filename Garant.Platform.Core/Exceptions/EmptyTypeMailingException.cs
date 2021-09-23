using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет если не был передан тип рассылки.
    /// </summary>
    public class EmptyTypeMailingException : Exception
    {
        public EmptyTypeMailingException() : base("Не передан тип рассылки")
        {

        }
    }
}
