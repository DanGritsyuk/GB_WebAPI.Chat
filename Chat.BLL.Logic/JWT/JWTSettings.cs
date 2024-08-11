using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HireWise.JWT
{
    public class JWTSettings
    {
        public string? SecretKey { get; set; }   // ключ для шифрации
        public string? Issuer { get; set; } // издатель токена
        public string? Audience { get; set; } // потребитель токена
        public int ExpiryMinutes { get; set; } // время жизни токена

        public SymmetricSecurityKey SymmetricSecurityKey // генерация симетричного ключа
        {
            get => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey!));
        }
    }
}