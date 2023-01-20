using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Garant.Platform.Core.Data
{
    public sealed class AuthOptions
    {
        public const string ISSUER = "GobizyServer"; // Издатель токена.
        public const string AUDIENCE = "GobizyClient"; // Потребитель токена.
        const string KEY = "mysup3rs3cr3t_s3cr3tkey!123";   // Ключ для шифрования.
        public const int LIFETIME = 15; // Время жизни токена.

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
