using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет, если не передан аккаунт пользователя.
    /// </summary>
    public class EmptyUserAccountException : Exception
    {
        public EmptyUserAccountException(string account) : base($"Пользователя с аккаунтом {account} не существует в системе")
        {
        }
    }
}