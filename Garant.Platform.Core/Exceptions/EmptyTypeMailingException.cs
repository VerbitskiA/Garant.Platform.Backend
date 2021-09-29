using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет если не переданы данные для рассылки.
    /// </summary>
    public class EmptyTypeMailingException : Exception
    {
        public EmptyTypeMailingException() : base("Не переданы данные для рассылки")
        {

        }
    }
}
