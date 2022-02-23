using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет, если не передан тип карточки.
    /// </summary>
    public class EmptyCardTypeException : Exception
    {
        public EmptyCardTypeException(string cardType) : base($"Недопустимый тип карточки. cardType был {cardType}")
        {
        }
    }
}