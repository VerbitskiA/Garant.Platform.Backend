using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет, если не было переданы доп. данные пользователя.
    /// </summary>
    public class EmptyUserInformationException : Exception
    {
        public EmptyUserInformationException() : base("Не переданы доп. данные пользователя")
        {

        }
    }
}
