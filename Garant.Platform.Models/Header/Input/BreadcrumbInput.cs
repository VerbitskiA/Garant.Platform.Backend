namespace Garant.Platform.Models.Header.Input
{
    /// <summary>
    /// Класс входной модели хлебных крошек.
    /// </summary>
    public class BreadcrumbInput
    {
        /// <summary>
        /// Селектор страницы, для которой нужно сформировать хлебные крошки.
        /// </summary>
        public string SelectorPage { get; set; }
    }
}
