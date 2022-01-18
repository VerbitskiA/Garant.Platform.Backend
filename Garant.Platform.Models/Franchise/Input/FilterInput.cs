namespace Garant.Platform.Models.Franchise.Input
{
    /// <summary>
    /// Класс входной модели для фильтрации франшиз.
    /// </summary>
    public class FilterInput
    {
        /// <summary>
        /// Тип фильтрации цены (по возрастанию или убыванию).
        /// </summary>
        public string TypeSortPrice { get; set; }

        /// <summary>
        /// Покупка через гарант.
        /// </summary>
        public bool IsGarant { get; set; }

        /// <summary>
        /// Прибыль в мес. от.
        /// </summary>
        public double ProfitMinPrice { get; set; }

        /// <summary>
        /// Прибыль в мес. до.
        /// </summary>
        public double ProfitMaxPrice { get; set; }

        /// <summary>
        /// Код бизнеса.
        /// </summary>
        public string ViewCode { get; set; }

        /// <summary>
        /// Код категории.
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Код города.
        /// </summary>
        //public string CityCode { get; set; }

        /// <summary>
        /// Цена инвестиций от.
        /// </summary>
        public double MinPriceInvest { get; set; }

        /// <summary>
        /// Цена инвестиций до.
        /// </summary>
        public double MaxPriceInvest { get; set; }
    }
}
