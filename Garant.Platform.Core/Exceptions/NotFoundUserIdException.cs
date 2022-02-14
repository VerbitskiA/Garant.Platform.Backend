using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если не было найдено Id пользователя по аккаунту.
    /// </summary>
    public class NotFoundUserIdException : Exception
    {
        public NotFoundUserIdException(string account) : base($"Пользователя с аккаунтом {account} не найдено")
        {
        }
    }
}