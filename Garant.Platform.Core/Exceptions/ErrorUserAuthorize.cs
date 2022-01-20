using System;

namespace Garant.Platform.Core.Exceptions
{
    public class ErrorUserAuthorize : Exception
    {
        public ErrorUserAuthorize(string email) : base($"Пользователя {email} не найдено")
        {

        }
    }
}
