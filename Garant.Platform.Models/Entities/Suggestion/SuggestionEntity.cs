using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.Suggestion
{
    /// <summary>
    /// Класс сопоставляется с таблицей предложений dbo.Suggestions.
    /// </summary>
    [Table("Suggestions", Schema = "dbo")]
    public class SuggestionEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long SuggestionId { get; set; }

        /// <summary>
        /// Текст описания.
        /// </summary>
        [Column("Text", TypeName = "varchar(200)")]
        public string Text { get; set; }

        /// <summary>
        /// Текст кнопки не интересно.
        /// </summary>
        [Column("Button1Text", TypeName = "varchar(50)")]
        public string Button1Text { get; set; }

        /// <summary>
        /// Текст кнопки подробнее.
        /// </summary>
        [Column("Button2Text", TypeName = "varchar(50)")]
        public string Button2Text { get; set; }

        /// <summary>
        /// Флаг скрытия блока.
        /// </summary>
        [Column("IsDisplay", TypeName = "bool")]
        public bool IsDisplay { get; set; }

        /// <summary>
        /// Id пользователя, которому не интересно предложение.
        /// </summary>
        [Column("UserId", TypeName = "text")]
        public string UserId { get; set; }

        /// <summary>
        /// Флаг нужно ли получить одно предложение с этим флагом.
        /// </summary>
        [Column("IsSingle", TypeName = "bool")]
        public bool IsSingle { get; set; }

        /// <summary>
        /// Флаг нужно ли получить все предложения с этим флагом.
        /// </summary>
        [Column("IsAll", TypeName = "bool")]
        public bool IsAll { get; set; }
    }
}
