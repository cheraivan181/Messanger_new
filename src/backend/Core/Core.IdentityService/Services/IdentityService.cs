using Core.CryptService.Interfaces;
using Core.IdentityService.Domain;
using Core.IdentityService.Domain.Options;
using Core.IdentityService.Interfaces;
using Core.Repositories.Interfaces;
using Core.Utils;
using Microsoft.Extensions.Options;
using Serilog;

namespace Core.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ISessionRepository _sessionRepository;

        private readonly IJwtService _jwtService;
        private readonly IHashService _hashService;

        private readonly IOptions<TokenLifeTimeOptions> _tokenLifeTimeOptions;

        public IdentityService(IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ISessionRepository sessionRepository,
            IJwtService jwtService,
            IHashService hashService,
            IOptions<TokenLifeTimeOptions> tokenLifeTimeOptions)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _sessionRepository = sessionRepository;
            _jwtService = jwtService;
            _hashService = hashService;
            _tokenLifeTimeOptions = tokenLifeTimeOptions;
        }

        public async Task<SignUpResult> SignUpAsync(string userName,
            string phone,
            string email,
            string password)
        {
            var result = new SignUpResult();
            var isUserExists = await _userRepository.CheckUserByUserNameAsync(userName);
            if (isUserExists)
            {
                result.SetError($"User with userName: {userName} is alredy exist");
                return result;
            }

            var role = await _roleRepository.GetRoleByNameAsync(CommonConstants.UserRole);
            if (role == null)
            {
                Log.Error($"Cannot find role with name #({CommonConstants.UserRole})");
                result.SetServerError();
                return result;
            }

            var hashPassword = _hashService.GetHash(password);
            var createdUserId = await _userRepository.CreateUserAsync(userName, phone, email, hashPassword);
            await _userRoleRepository.AddUserRoleAsync(createdUserId, role.Id);

            string refreshToken = _jwtService.GenarateRefreshToken();
            var createRefreshTokenResult = await _refreshTokenRepository.CreateRefreshTokenAsync(createdUserId, refreshToken);

            if (!createRefreshTokenResult)
            {
                Log.Error($"Cannot create refresh token for user#({createdUserId})");
                result.SetServerError();
                return result;
            }

            var roles = new List<string> { role.Name };
            var jwtToken = _jwtService.GenereteJwtToken(userName, createdUserId, roles);

            result.SetSucessResult(jwtToken, refreshToken);

            return result;
        }

        public async Task<SignInResult> SignInAsync(string userName, string password, Guid? sessionId)
        {
            var result = new SignInResult();

            var hashPassword = _hashService.GetHash(password);
            var user = await _userRepository.GetUserByUserNameAndPassword(userName, hashPassword);
            if (user == null)
            {
                result.SetUnAuthorizedError("Cannot find user with this username and password");
                return result;
            }
            
            result = await SignInAsync(result, user.Id, userName, sessionId);
            return result;
        }

        public async Task<SignInResult> UpdateJwtAsync(string refreshTokenValue, Guid? sessionId)
        {
            var result = new SignInResult();
            var refreshToken = await _refreshTokenRepository.GetRefreshTokenByValueAsync(refreshTokenValue);
            if (refreshToken == null)
            {
                result.SetUnAuthorizedError($"Cannot find user with refreshToken {refreshToken}");
                return result;
            }

            if (refreshToken.CreatedAt.AddYears(_tokenLifeTimeOptions.Value.RefreshTokenLifeTime) < DateTime.Now)
            {
                result.SetUnAuthorizedError("Token is expired");
                return result;
            }

            result = await SignInAsync(result , refreshToken.User.Id, refreshToken.User.UserName, sessionId,
                oldRefreshToken: refreshTokenValue);
            return result;
        }

        private async Task<SignInResult> SignInAsync(SignInResult result, Guid userId, 
            string userName, Guid? sessionId, string oldRefreshToken = "")
        {
            var refreshToken = _jwtService.GenarateRefreshToken();
            bool refreshTokenResult;
            
            if (string.IsNullOrEmpty(oldRefreshToken))
                refreshTokenResult = await _refreshTokenRepository.CreateRefreshTokenAsync(userId, refreshToken);
            else
                refreshTokenResult = await _refreshTokenRepository.UpdateRefreshTokenAsync(userId, oldRefreshToken, refreshToken);    
            
            if (!refreshTokenResult)
            {
                Log.Error($"Cannot create or update refresh token for user#({userId})");
                result.SetServerError();
                return result;
            }

            var roles = await _userRoleRepository.GetUserRolesAsync(userId);
            if (roles == null
                || roles.Count == 0)
            {
                Log.Error($"Cannot get user roles, user #({userId})");
                result.SetServerError();
            }

            string jwtToken;
            if (sessionId.HasValue)
            {
                var session = await _sessionRepository.GetSessionAsync(sessionId.Value);
                if (session == null)
                {
                    Log.Error($"Cannot find session #({sessionId.Value})");
                    result.SetError("Cannot find session");
                }

                if (session.UserId != userId)
                {
                    Log.Error($"User try use not persionalized session. User #({userId}), Session #({sessionId.Value})");
                    result.SetError("Incorrect session value");
                }
                
                jwtToken = _jwtService.GenereteJwtToken(userName, userId, roles.Select(x => x.Name)
                    .ToList(), sessionId.Value);
            }
            else 
                jwtToken = _jwtService.GenereteJwtToken(userName, userId, roles.Select(x => x.Name)
                    .ToList());

            result.SetSucessResult(jwtToken, refreshToken);
            return result;
        }
    }
}
