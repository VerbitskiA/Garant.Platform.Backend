using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Garant.Platform.Models.Entities.User;

namespace Garant.Platform.Models.Entities.Document
{
    /// <summary>
    /// Класс сопоставляется с таблицей документов Documents.Documents.
    /// </summary>
    [Table("Documents", Schema = "Documents")]
    public class DocumentEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        [Column("DocumentId", TypeName = "bigserial")]
        public long DocumentId { get; set; }

        /// <summary>
        /// Название документа.
        /// </summary>
        [Column("DocumentName", TypeName = "varchar(500)")]
        public string DocumentName { get; set; }

        /// <summary>
        /// Id предмета сделки (франшизы или бизнеса), которому принадлежит документ.
        /// </summary>
        [Column("DocumentItemId", TypeName = "bigint")]
        public long DocumentItemId { get; set; }

        /// <summary>
        /// Id пользователя, который прикрепил документ.
        /// </summary>
        [ForeignKey("Id")]
        [Column("UserId", TypeName = "text")]
        public string UserId { get; set; }

        /// <summary>
        /// Тип документа.
        /// </summary>
        [Column("DocumentType", TypeName = "varchar(150)")]
        public string DocumentType { get; set; }

        /// <summary>
        /// Дата прикрепления документа.
        /// </summary>
        [Column("DateCreate", TypeName = "timestamp")]
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Флаг документа сделки.
        /// </summary>
        [Column("IsDealDocument", TypeName = "bool")]
        public bool IsDealDocument { get; set; }

        /// <summary>
        /// Флаг подтвержден ли документ сделки.
        /// </summary>
        [Column("IsApproveDocument", TypeName = "bool")]
        public bool? IsApproveDocument { get; set; }

        /// <summary>
        /// Флаг отклонен ли документ сделки.
        /// </summary>
        [Column("IsRejectDocument", TypeName = "bool")]
        public bool? IsRejectDocument { get; set; }

        /// <summary>
        /// Причина отклонения.
        /// </summary>
        [Column("CommentReject", TypeName = "text")]
        public string CommentReject { get; set; }

        /// <summary>
        /// Отправлен ли документ продавцу.
        /// </summary>
        [Column("IsSend", TypeName = "bool")]
        public bool? IsSend { get; set; }

        public UserEntity User { get; set; }
    }
}
