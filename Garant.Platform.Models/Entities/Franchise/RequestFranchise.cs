using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Garant.Platform.Models.Entities.User;

namespace Garant.Platform.Models.Entities.Franchise
{
    /// <summary>
    /// Класс сопоставляется с таблицей списком заявок для франшиз Franchises.RequestsFranchises.
    /// </summary>
    [Table("RequestsFranchises", Schema = "Franchises")]
    public class RequestFranchise
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        [Column("RequestId", TypeName = "bigserial")]
        public long RequestId { get; set; }

        /// <summary>
        /// Id пользователя, который оставил заявку.
        /// </summary>
        [ForeignKey("Id")]
        [Column("UserId", TypeName = "text")]
        public string UserId { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        [Column("UserName", TypeName = "text")]
        public string UserName { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        [Column("Phone", TypeName = "varchar(150)")]
        public string Phone { get; set; }

        /// <summary>
        /// Город.
        /// </summary>
        [Column("City", TypeName = "varchar(200)")]
        public string City { get; set; }

        /// <summary>
        /// Дата создания заявки.
        /// </summary>
        [Column("DateCreate", TypeName = "timestamp")]
        public DateTime DateCreate { get; set; }

        public UserEntity User { get; set; }
    }
}
