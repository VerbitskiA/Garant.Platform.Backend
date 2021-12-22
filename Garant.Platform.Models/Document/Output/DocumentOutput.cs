using System;

namespace Garant.Platform.Models.Document.Output
{
    /// <summary>
    /// Класс выходной модели документов сделки.
    /// </summary>
    public class DocumentOutput
    {
        /// <summary>
        /// Id документа.
        /// </summary>
        public long DocumentId { get; set; }

        /// <summary>
        /// Название документа.
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Id франшизы или бизнеса, к которому принадлежит документ.
        /// </summary>
        public long DocumentItemId { get; set; }

        /// <summary>
        /// Id пользователя, который прикрепил документ.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Тип документа.
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Дата прикрепления документа.
        /// </summary>
        public string DateCreate { get; set; }

        /// <summary>
        /// Флаг документа сделки.
        /// </summary>
        public bool IsDealDocument { get; set; }

        /// <summary>
        /// Флаг подтвержден ли документ сделки.
        /// </summary>
        public bool? IsAcceptDocument { get; set; }
    }
}
