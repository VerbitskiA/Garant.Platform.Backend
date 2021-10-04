using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.User
{
    /// <summary>
    /// Класс сопоставляется с таблицей информации о пользователе dbo.UsersInformation.
    /// </summary>
    [Table("UsersInformation", Schema = "dbo")]
    public class UserInformationEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long InformationId { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [Column("FirstName", TypeName = "varchar(200)")]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        [Column("LastName", TypeName = "varchar(200)")]
        public string LastName { get; set; }

        /// <summary>
        /// Город.
        /// </summary>
        [Column("City", TypeName = "varchar(100)")]
        public string City { get; set; }

        /// <summary>
        /// Почта.
        /// </summary>
        [Column("Email", TypeName = "varchar(200)")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [Column("Password", TypeName = "varchar(200)")]
        public string Password { get; set; }

        /// <summary>
        /// Название причины регистрации.
        /// </summary>
        [Column("VariantForWhatName", TypeName = "varchar(100)")]
        public string VariantForWhatName { get; set; }

        /// <summary>
        /// Код причины регистрации.
        /// </summary>
        [Column("VariantForWhatCode", TypeName = "varchar(100)")]
        public string VariantForWhatCode { get; set; }
    }
}
