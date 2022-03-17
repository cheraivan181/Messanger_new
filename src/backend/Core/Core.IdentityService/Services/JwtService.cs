using Core.Identity;
using Core.IdentityService.Domain.Options;
using Core.IdentityService.Interfaces;
using Core.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.IdentityService.Services
{
    public class JwtService : IJwtService
    {
        private readonly IOptions<TokenLifeTimeOptions> _tokenLifeTimeOptions;
        private readonly IOptions<TokenOptions> _tokenOptions;
        private readonly IOptions<AuthOptions> _authOptions;

        public JwtService(IOptions<TokenLifeTimeOptions> tokenLifeTimeOptions,
            IOptions<TokenOptions> tokenOptions,
            IOptions<AuthOptions> authOptions)
        {
            _tokenLifeTimeOptions = tokenLifeTimeOptions;
            _tokenOptions = tokenOptions;
            _authOptions = authOptions;
        }

        public string GenarateRefreshToken()    
        {
            return CryptoRandomizer.GetRandomString(32);
        }

        public string GenereteJwtToken(string userName, 
            long userid, 
            List<string> roles,
            string sessionId = "")
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userid.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(CommonConstants.UniqueClaimName, CryptoRandomizer.GetRandomString(16))
            };

            if (!string.IsNullOrEmpty(sessionId))
                claims.Add(new Claim(CommonConstants.SessionClaimName, sessionId));
            
            foreach (var role in roles)
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));

            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_tokenLifeTimeOptions.Value.AccessTokenLifeTime));
            var now = DateTime.Now;

            var signInEncodingKey = new SignInSymmetricKey(_tokenOptions.Value.Key);

            var signInCreditials = new SigningCredentials(signInEncodingKey.GetKey(),
              signInEncodingKey.SignInAlgorithm);

            var token = new JwtSecurityToken(
                    _authOptions.Value.Issuer,
                    _authOptions.Value.Audience,
                    notBefore: now,
                    claims: claims,
                    expires: expires,
                    signingCredentials: signInCreditials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
