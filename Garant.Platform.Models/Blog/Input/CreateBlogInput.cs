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
        /// Путь к изображению.
        /// </summary>
        public string Url { get; set; }    

    }
}
