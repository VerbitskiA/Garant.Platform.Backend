using System;
using System.Collections.Generic;
using Garant.Platform.Models.Franchise.Other;

namespace Garant.Platform.Models.Franchise.Output
{
    public class FranchiseOutput
    {
        /// <summary>
        /// Путь к изображению.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст описания.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Текст до цены.
        /// </summary>
        public string TextDoPrice { get; set; }

        /// <summary>
        /// Кол-во дней.
        /// </summary>
        public int CountDays { get; set; }

        /// <summary>
        /// Склонение дней.
        /// </summary>
        public string DayDeclination { get; set; }

        /// <summary>
        /// Категория франшизы.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Подкатегория.
        /// </summary>
        public string SubCategory { get; set; }

        public List<SubCategory> SubCategoryResult { get; set; } = new();

        /// <summary>
        /// Полная строка текста для вставки в одном поле.
        /// </summary>
        public string FullText { get; set; }
    }
}
