using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет если не был передан
    /// </summary>
    public class EmptyPhoneNumberException : Exception
    {
        public EmptyPhoneNumberException() : base("Не передан номер телефона")
        {

        }
    }
}
