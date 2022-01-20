using System;

namespace Garant.Platform.Commerce.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, ели не был найден заказ для его обновления.
    /// </summary>
    public class ErrorUpdateSystemOrderIdException : Exception
    {
        private readonly object id;
        private readonly string message;

        public ErrorUpdateSystemOrderIdException(object _id, string _message)
        {
            id = _id;
            message = _message;
        }

        public override string ToString()
        {
            return $"{message} " + id;
        }
    }
}
