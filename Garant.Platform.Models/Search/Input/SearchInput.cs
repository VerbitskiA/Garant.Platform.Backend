namespace Garant.Platform.Models.Search.Input
{
    /// <summary>
    /// Класс входной модели для поиска.
    /// </summary>
    public class SearchInput
    {
        /// <summary>
        /// Тип поиска. 
        /// </summary>
        public string SearchType { get; set; }

        /// <summary>
        /// Текст поиска.
        /// </summary>
        public string SearchText { get; set; }
    }
}
