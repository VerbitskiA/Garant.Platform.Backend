namespace Garant.Platform.Models.Header.Output
{
    /// <summary>
    /// Класс выходной модели хлебных крошек.
    /// </summary>
    public class BreadcrumbOutput
    {
        /// <summary>
        /// Название пункта.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Селектор страницы, для которой нужно сформировать хлебные крошки.
        /// </summary>
        public string SelectorPage { get; set; }

        /// <summary>
        /// Номер позиции в списке.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Флаг текущего пункта.
        /// </summary>
        public bool IsCurrent { get; set; }
    }
}
