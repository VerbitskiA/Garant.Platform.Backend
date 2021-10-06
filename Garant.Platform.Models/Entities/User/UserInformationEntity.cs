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
        /// Номер телефона.
        /// </summary>
        [Column("PhoneNumber", TypeName = "varchar(100)")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Значения разделенные запятой.
        /// </summary>
        [Column("Values", TypeName = "varchar(50)")]
        public string Values { get; set; }

        /// <summary>
        /// Id пользователя.
        /// </summary>
        [Column("UserId", TypeName = "varchar(50)")]
        public string UserId { get; set; }
    }
}
