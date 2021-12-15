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
        /// Id франшизы или бизнеса, к которому принадлежит документ.
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

        public UserEntity User { get; set; }
    }
}
