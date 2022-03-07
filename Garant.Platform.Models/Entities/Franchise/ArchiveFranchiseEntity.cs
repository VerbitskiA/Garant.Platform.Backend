using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Franchise
{
    /// <summary>
    /// Класс сопоставляется с таблицей заархивированных франшиз Franchises.ArchiveFranchises.
    /// </summary>
    [Table("ArchiveFranchises", Schema = "Franchises")]
    public class ArchiveFranchiseEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long ArchiveFranchiseId { get; set; }

        /// <summary>
        /// Идентификатор франшизы
        /// </summary>
        [Column("FranchiseId", TypeName = "int")]
        public long FranchiseId { get; set; }

        /// <summary>
        /// Архивирована ли франшиза.
        /// </summary>
        [Column("IsArchive", TypeName = "boolean")]
        public bool IsArchive { get; set; }

        /// <summary>
        /// Дата помещения в архив.
        /// </summary>
        [Column("ArchivingDate", TypeName = "timestamp")]
        public DateTime ArchivingDate { get; set; }
    }
}
