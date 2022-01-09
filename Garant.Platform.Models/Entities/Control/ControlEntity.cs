using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Control
{
    /// <summary>
    /// Класс сопоставляется с таблицей dbo.Controls.
    /// </summary>
    [Table("Controls", Schema = "dbo")]
    public class ControlEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key] 
        [Column("ControlId", TypeName = "serial")]
        public int ControlId { get; set; }

        /// <summary>
        /// Тип контрола.
        /// </summary>
        [Column("ControlType", TypeName = "varchar(150)")]
        public string ControlType { get; set; }

        /// <summary>
        /// Название контрола.
        /// </summary>
        [Column("ControlName", TypeName = "varchar(150)")]
        public string ControlName { get; set; }

        /// <summary>
        /// Код Guid.
        /// </summary>
        [Column("ValueId", TypeName = "text")]
        public Guid ValueId { get; set; }

        /// <summary>
        /// Значение.
        /// </summary>
        [Column("Value", TypeName = "varchar(400)")]
        public string Value { get; set; }

        /// <summary>
        /// Позиция в списке.
        /// </summary>
        [Column("Position", TypeName = "int")]
        public int Position { get; set; }

        /// <summary>
        /// Выбрано ли по дефолту.
        /// </summary>
        [Column("IsDefault", TypeName = "bool")]
        public bool IsDefault { get; set; }
    }
}
