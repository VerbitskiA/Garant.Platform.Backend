using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Configurator
{
    /// <summary>
    /// Класс сопоставляется с таблицей действий для блогов.
    /// </summary>
    [Table("BlogActions", Schema = "Configurator")]
    public class BlogActionEntity
    {
        [Key]
        public int BlogActionId { get; set; }

        /// <summary>
        /// Название действия.
        /// </summary>
        public string BlogActionName { get; set; }

        /// <summary>
        /// Системное название действия.
        /// </summary>
        public string BlogActionSysName { get; set; }
    }
}