using System;

namespace Garant.Platform.Models.LastBuy.Output
{
    /// <summary>
    /// Класс выходной модели последних купленных франшиз.
    /// </summary>
    public class LastBuyOutput
    {
        /// <summary>
        /// Путь
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Текст описания.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Текст до цены.
        /// </summary>
        public string TextDoPrice { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Дата покупки.
        /// </summary>
        public DateTime DateBuy { get; set; }

        /// <summary>
        /// Кол-во дней.
        /// </summary>
        public int CountDays { get; set; }

        /// <summary>
        /// Склонение дней.
        /// </summary>
        public string DayDeclination { get; set; }
    }
}
