using System;

namespace Garant.Platform.Models.Blog.Output
{
    /// <summary>
    /// Класс выходной модели создания статьи блога.
    /// </summary>
    public class ArticleThemeOutput
    {
        public long ThemeId { get; set; }

        /// <summary>
        /// Код темы статьи.
        /// </summary>
        public Guid ThemeCode { get; set; }

        /// <summary>
        /// Название темы статьи.
        /// </summary>
        public string ThemeName { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        public int Position { get; set; }
    }
}