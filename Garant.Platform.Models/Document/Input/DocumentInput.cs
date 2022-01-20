namespace Garant.Platform.Models.Document.Input
{
    /// <summary>
    /// Класс входной модели документов сделки.
    /// </summary>
    public class DocumentInput
    {
        /// <summary>
        /// Id предмета сделки (франшизы или бизнеса).
        /// </summary>
        public long DocumentItemId { get; set; }

        /// <summary>
        /// Флаг документа сделки.
        /// </summary>
        public bool IsDealDocument { get; set; }

        /// <summary>
        /// Тип документа.
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Номер этапа.
        /// </summary>
        public int Iteration { get; set; }
    }
}
