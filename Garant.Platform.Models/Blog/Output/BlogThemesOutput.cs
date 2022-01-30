
using System;

namespace Garant.Platform.Models.Blog.Output
{
    /// <summary>
    /// Класс выходной модели 
    /// </summary>
    public class BlogThemesOutput
    {
        /// <summary>
        /// Заголовок, название темы.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Код категории темы блога (GUID).
        /// </summary>
        public string ThemeCategoryCode { get; set; }
        
        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime DateCreated { get; set; }
    }
}
