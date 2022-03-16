using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет, если не переданы все обязательные параметры для заявки посадочной страницы.
    /// </summary>
    public class EmptyRequestLandingParamsException : Exception
    {
        public EmptyRequestLandingParamsException() : base("Не заполнено имя или номер телефона.")
        {
        }
    }
}