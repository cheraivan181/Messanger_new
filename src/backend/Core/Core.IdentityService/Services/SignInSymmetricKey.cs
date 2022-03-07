using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Core.Identity
{
    public class SignInSymmetricKey
    {
        private readonly SymmetricSecurityKey secretKey;
        public string SignInAlgorithm { get; } = SecurityAlgorithms.HmacSha256;

        public SignInSymmetricKey(string key)
        {
            secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public SecurityKey GetKey() =>
            secretKey;
        
    }
}
