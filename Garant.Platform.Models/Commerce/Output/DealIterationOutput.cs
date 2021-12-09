namespace Garant.Platform.Models.Commerce.Output
{
    /// <summary>
    /// Класс выходной модели этапов сделки.
    /// </summary>
    public class DealIterationOutput
    {
        /// <summary>
        /// Номер этапа.
        /// </summary>
        public int NumberIteration { get; set; }

        /// <summary>
        /// Фоаг завершен ли этап.
        /// </summary>
        public bool IsCompletedIteration { get; set; }

        /// <summary>
        /// Название этапа.
        /// </summary>
        public string IterationName { get; set; }

        /// <summary>
        /// Позиция.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// FK на сделку.
        /// </summary>
        public long DealIteration { get; set; }

        /// <summary>
        /// Описание этапа.
        /// </summary>
        public string IterationDetail { get; set; }
    }
}
