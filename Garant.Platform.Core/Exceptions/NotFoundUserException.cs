using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если не было найдено пользователя с таким Id.
    /// </summary>
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException(string Id) : base($"Пользователя с {nameof(Id)}: {Id} не найдено!")
        {

        }
    }
}
