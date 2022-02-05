using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Models.Entities.Configurator
{
    /// <summary>
    /// Класс сопоставляется с таблицей списка меню конфигуратора.
    /// </summary>
    [Keyless]
    [Table("ConfiguratorMenuItems", Schema = "Configurator")]
    public class ConfiguratorMenuEntity
    {
        /// <summary>
        /// Id элемента меню.
        /// </summary>
        public int MenuItemId { get; set; }

        /// <summary>
        /// Название пункта меню.
        /// </summary>
        [Column("MenuItemName", TypeName = "varchar(200)")]
        public string MenuItemName { get; set; }

        /// <summary>
        /// Системное название пункта меню.
        /// </summary>
        [Column("MenuItemSysName", TypeName = "varchar(250)")]
        public string MenuItemSysName { get; set; }
        
        /// <summary>
        /// Название действия при нажатии на пункт меню.
        /// </summary>
        [Column("ActionName", TypeName = "varchar(250)")]
        public string ActionName { get; set; }

        /// <summary>
        /// Номер позиции в списке.
        /// </summary>
        [Column("Position", TypeName = "int")]
        public int Position { get; set; }
    }
}