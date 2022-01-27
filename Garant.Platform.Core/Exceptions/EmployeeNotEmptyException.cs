using System;

namespace Garant.Platform.Configurator.Exceptions
{
    /// <summary>
    /// Исключение возникает, если такой сотрудник уже заведен в системе.
    /// </summary>
    public class EmployeeNotEmptyException : Exception
    {
        public EmployeeNotEmptyException(string email, string phoneNumber) : base(
            $"Сотрудник с почтой {email} и номером телефона {phoneNumber} уже заведен в системе")
        {
        }
    }
}