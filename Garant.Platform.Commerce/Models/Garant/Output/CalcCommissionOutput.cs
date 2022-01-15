namespace Garant.Platform.Commerce.Models.Garant.Output
{
    /// <summary>
    /// Класс выходной модели комиссии.
    /// </summary>
    public class CalcCommissionOutput
    {
        /// <summary>
        /// Сумма до вычета комиссии.
        /// </summary>
        public double BeforeCalcAmount { get; set; }

        /// <summary>
        /// Сумма после вычета комиссии.
        /// </summary>
        public double AfterCalcAmount { get; set; }
    }
}
