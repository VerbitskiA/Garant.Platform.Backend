using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет, если не переданы все параметры для заявки.
    /// </summary>
    public class EmptyRequestInputParamsException : Exception
    {
        public EmptyRequestInputParamsException(string id, string type) : base($"Не переданы все обязательные параметры. Id было {id}. Тип был {type}")
        {
        }
    }
}