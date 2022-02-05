using System;

namespace Garant.Platform.Models.Blog.Input
{
    /// <summary>
    /// Класс входной модели создания статьи блога.
    /// </summary>
    public class ArticleThemeInput
    {
        /// <summary>
        /// Код темы статьи.
        /// </summary>
        public Guid ThemeCode { get; set; }
    }
}