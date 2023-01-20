using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garant.Platform.Models.Entities.User
{
    [Table("RefreshTokens", Schema = "dbo")]
    public class RefreshTokenEntity
    {
        /// <summary>
        /// Идентификатор обновления токена.
        /// </summary>
        [Key]
        [Column("RefreshTokenId")]
        public Guid RefreshTokenId { get; set; }

        /// <summary>
        /// Токен.
        /// </summary>
        [Column("RefreshToken")]
        public string Token { get; set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        [Column("UserId")]
        public string UserId { get; set; }
    }
}
