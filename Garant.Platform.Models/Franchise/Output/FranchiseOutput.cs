using System;

namespace Garant.Platform.Models.Franchise.Output
{
    public class FranchiseOutput
    {
        /// <summary>
        /// Id франшизы.
        /// </summary>
        public long FranchiseId { get; set; }

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

        //public List<SubCategory> SubCategoryResult { get; set; } = new();

        /// <summary>
        /// Полная строка текста для вставки в одном поле.
        /// </summary>
        public string FullText { get; set; }

        /// <summary>
        /// Покупка через гарант.
        /// </summary>
        public bool IsGarant { get; set; }

        /// <summary>
        /// Желаемая прибыль в мес.
        /// </summary>
        public double ProfitPrice { get; set; }

        /// <summary>
        /// Режим просмотр или редактирование франшизы.
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// Сумма инвестиций.
        /// </summary>
        public string TotalInvest { get; set; }
    }
}
