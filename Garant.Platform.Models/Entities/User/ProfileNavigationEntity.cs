using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.User
{
    /// <summary>
    /// Класс сопоставляется с таблицей навигации ЛК.
    /// </summary>
    [Table("ProfileNavigations", Schema = "dbo")]
    public class ProfileNavigationEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int NavigationId { get; set; }

        /// <summary>
        /// Название пункта навигации.
        /// </summary>
        [Column("NavigationText", TypeName = "varchar(200)")]
        public string NavigationText { get; set; }

        /// <summary>
        /// Ссылка на страницу.
        /// </summary>
        [Column("NavigationLink", TypeName = "varchar(300)")]
        public string NavigationLink { get; set; }

        /// <summary>
        /// Флаг видимости пункта.
        /// </summary>
        [Column("IsHide", TypeName = "bool")]
        public bool IsHide { get; set; }

        /// <summary>
        /// Позиция в списке.
        /// </summary>
        [Column("Position", TypeName = "int")]
        public int Position { get; set; }
    }
}
