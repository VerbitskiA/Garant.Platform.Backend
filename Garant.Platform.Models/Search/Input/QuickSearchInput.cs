namespace Garant.Platform.Models.Search.Input
{
    /// <summary>
    /// Класс входной модели для областей быстрого поиска.
    /// </summary>
    public class QuickSearchInput
    {
        /// <summary>
        /// Код бизнеса.
        /// </summary>
        public string viewCode { get; set; }

        /// <summary>
        /// Код категории.
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Код города.
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// Цена от.
        /// </summary>
        public double MinPrice { get; set; }

        /// <summary>
        /// Цена до.
        /// </summary>
        public double MaxPrice { get; set; }
    }
}
