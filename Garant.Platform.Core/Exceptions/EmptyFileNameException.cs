using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет, если не было передано имя файла.
    /// </summary>
    public class EmptyFileNameException : Exception
    {
        public EmptyFileNameException() : base("Не передано имя файла")
        {
        }
    }
}