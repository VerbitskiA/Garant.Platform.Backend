using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Garant.Platform.Models.Entities.User;

namespace Garant.Platform.Models.Entities.Business
{
    /// <summary>
    /// Класс сопоставляется с таблицей списком заявок для бизнеса Business.RequestsBusinesses.
    /// </summary>
    [Table("RequestsBusinesses", Schema = "Business")]
    public class RequestBusinessEntity
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
        [Column("UserName", TypeName = "varchar(150)")]
        public string UserName { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        [Column("Phone", TypeName = "varchar(150)")]
        public string Phone { get; set; }

        /// <summary>
        /// Дата создания заявки.
        /// </summary>
        [Column("DateCreate", TypeName = "timestamp")]
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Id бизнеса, по которому оставлена заявка.
        /// </summary>
        [ForeignKey("BusinessId")]
        [Column("BusinessId", TypeName = "bigint")]
        public long BusinessId { get; set; }

        /// <summary>
        /// Статус заявки.
        /// </summary>
        [Column("RequestStatus", TypeName = "varchar(100)")]
        public string RequestStatus { get; set; }

        public UserEntity User { get; set; }

        public BusinessEntity Business { get; set; }
    }
}
