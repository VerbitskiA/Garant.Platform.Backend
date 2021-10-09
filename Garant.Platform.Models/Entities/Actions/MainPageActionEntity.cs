using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Actions
{
    /// <summary>
    /// Класс сопоставляется с таблицей событий на главной странице dbo.MainPageActions.
    /// </summary>
    [Table("MainPageActions", Schema = "dbo")]
    public class MainPageActionEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public int ActionId { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [Column("Title", TypeName = "varchar(200)")]
        public string Title { get; set; }

        /// <summary>
        /// Подзаголовок.
        /// </summary>
        [Column("SubTitle", TypeName = "varchar(200)")]
        public string SubTitle { get; set; }

        /// <summary>
        /// Текст описания.
        /// </summary>
        [Column("Text", TypeName = "varchar(500)")]
        public string Text { get; set; }

        /// <summary>
        /// Текст кнопки.
        /// </summary>
        [Column("ButtonText", TypeName = "varchar(50)")]
        public string ButtonText { get; set; }
    }
}
