using System;

namespace Garant.Platform.Models.Franchise.Output
{
    public class PopularFranchiseOutput
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
        /// Сумма инвестиций.
        /// </summary>
        public string TotalInvest { get; set; }

        /// <summary>
        /// Id франшизы.
        /// </summary>
        public long FranchiseId { get; set; }
    }
}
