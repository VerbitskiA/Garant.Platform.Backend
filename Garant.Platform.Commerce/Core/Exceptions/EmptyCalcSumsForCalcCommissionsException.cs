using System;

namespace Garant.Platform.Commerce.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет, если какая-либо из сумм для вычисления комиссии <= 0.
    /// </summary>
    public class EmptyCalcSumsForCalcCommissionsException : Exception
    {
        public EmptyCalcSumsForCalcCommissionsException() : base("Не заполнена какая-либо из сумм, необходимых для вычисления комиссии сервиса.")
        {
        }
    }
}
