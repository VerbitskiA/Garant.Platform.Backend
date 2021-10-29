using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Garant.Platform.Models.Entities.User;

namespace Garant.Platform.Models.Entities.Franchise
{
    /// <summary>
    /// Класс сопоставляется с временной таблицей франшиз.
    /// </summary>
    [Table("TempFranchises", Schema = "Franchises")]
    public class TempFranchiseEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long TempId { get; set; }

        /// <summary>
        /// Id пользователя.
        /// </summary>
        [Column("UserId", TypeName = "text")]
        public string Id { get; set; }

        [ForeignKey("Id")]
        public UserEntity User { get; set; }

        /// <summary>
        /// Название файла.
        /// </summary>
        [Column("FileName", TypeName = "varchar(300)")]
        public string FileName { get; set; }
    }
}
