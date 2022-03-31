using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ConnectionHandler.Auth;

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