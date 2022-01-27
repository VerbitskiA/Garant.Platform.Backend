using System;

namespace Garant.Platform.Configurator.Exceptions
{
    /// <summary>
    /// Исключение возникает, если не все обязательные поля сотрудника заполнены.
    /// </summary>
    public class EmptyEmployeeDataException : Exception
    {
        public EmptyEmployeeDataException() : base("Не все обязательные поля сотрудника заполнены")
        {
        }
    }
}