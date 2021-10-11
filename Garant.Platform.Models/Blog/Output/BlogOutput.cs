namespace Garant.Platform.Models.Blog.Output
{
    /// <summary>
    /// Класс выходной модели 
    /// </summary>
    public class BlogOutput
    {
        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Путь к изображению.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Оплачено ли размещение на главной.
        /// </summary>
        public bool IsPaid { get; set; }
    }
}
