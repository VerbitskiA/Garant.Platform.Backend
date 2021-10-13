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
        public string ProfitMinPrice { get; set; }

        /// <summary>
        /// Прибыль в мес. до.
        /// </summary>
        public string ProfitMaxPrice { get; set; }
    }
}
