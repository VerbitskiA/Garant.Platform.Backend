using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет, если не найдено информации по пользователю.
    /// </summary>
    public class NotFoundUserInfoException : Exception
    {
        public NotFoundUserInfoException(string account) : base($"Информации о пользователе {account} не найдено в системе")
        {
        }
    }
}