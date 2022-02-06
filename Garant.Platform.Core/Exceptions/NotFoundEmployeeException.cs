using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если не было найдено такого сотрудника в системе.
    /// </summary>
    public class NotFoundEmployeeException : Exception
    {
        public NotFoundEmployeeException() : base("Такого сотрудника не существует в системе")
        {
        }
    }
}