using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Logger
{
    /// <summary>
    /// Класс сопоставляется с таблицей логов Logs.Logs.
    /// </summary>
    [Table("Logs", Schema = "Logs")]
    public class LoggerEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key, Column("LogId", TypeName = "bigserial")]
        public int LogId { get; set; }

        /// <summary>
        /// Уровень логгирования.
        /// </summary>
        [Column("LogLvl", TypeName = "varchar(100)")]
        public string LogLvl { get; set; }  

        /// <summary>
        /// Тип исключения.
        /// </summary>
        [Column("TypeException", TypeName = "varchar(100)")]
        public string TypeException { get; set; }   

        /// <summary>
        /// Сообщение исключения.
        /// </summary>
        [Column("Exception", TypeName = "text")]
        public string Exception { get; set; }   

        /// <summary>
        /// Путь, где возникло исключение.
        /// </summary>
        [Column("StackTrace", TypeName = "text")]
        public string StackTrace { get; set; }  

        /// <summary>
        /// Дата создания лога.
        /// </summary>
        [Column("Date", TypeName = "timestamp")]
        public DateTime Date { get; set; }  
    }
}
