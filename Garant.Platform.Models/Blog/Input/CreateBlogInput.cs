namespace Garant.Platform.Models.Blog.Input
{
    /// <summary>
    /// Класс входной модели блога.
    /// </summary>
    public class CreateBlogInput
    {
        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Код темы блога.
        /// </summary>
        public string ThemeCategoryCode { get; set; }

    }
}
