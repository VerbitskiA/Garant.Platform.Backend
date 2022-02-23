using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет, если не был передан Id карточки.
    /// </summary>
    public class EmptyCardIdException : Exception
    {
        public EmptyCardIdException(long cardId) : base($"Недопустимое значение Id арточки. Id карточки было {cardId}")
        {
        }
    }
}