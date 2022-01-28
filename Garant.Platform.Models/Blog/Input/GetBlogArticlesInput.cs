namespace Garant.Platform.Models.Blog.Input
{
    /// <summary>
    /// Класс входной модели для получение статей блога.
    /// </summary>
    public class GetBlogArticlesInput
    {
        /// <summary>
        /// Id блога, для которого нужно получить список статей.
        /// </summary>
        public long BlogId { get; set; }
    }
}