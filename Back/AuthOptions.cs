using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Back
{
    public class AuthOptions
    {
        public const string ISSUER = @"https://localhost:5001/"; // издатель токена
        public const string AUDIENCE = @"https://localhost:5001/"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123456";   // ключ для шифрации
        public const int LIFETIME = 120; // время жизни токена в минутах
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
