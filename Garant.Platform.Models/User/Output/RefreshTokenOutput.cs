using System;

namespace Garant.Platform.Models.User.Output
{
    public class RefreshTokenOutput
    {
        public Guid TokenId { get; set; }

        public string Token { get; set; }

        public string UserId { get; set; }
    }
}
