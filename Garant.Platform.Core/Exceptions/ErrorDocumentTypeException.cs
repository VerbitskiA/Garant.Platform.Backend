using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникает, если передан не тот тип документа.
    /// </summary>
    public class ErrorDocumentTypeException : Exception
    {
        public ErrorDocumentTypeException(string documentType) : base($"Тип документа не соответствует типу {documentType}")
        {

        }
    }
}
