using Core.CryptService.Interfaces;
using Core.IdentityService.Domain.Options;
using Core.IdentityService.Interfaces;
using Core.IdentityService.Services;
using Core.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Core.IdentityService.UnitTests
{
    public class IdentityServiceTests
    {
        private readonly IIdentityService _identityService;
        private readonly IHashService _hashService;

        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IRoleRepository> _roleRepository;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
        private readonly Mock<IUserRoleRepository> _userRoleRepository;

        private readonly IJwtService _jwtService;

        private readonly Mock<IOptions<TokenLifeTimeOptions>> _tokenLifeTimeOptions;
        private readonly Mock<IOptions<TokenOptions>> _tokenOptions;
        private readonly Mock<IOptions<AuthOptions>> _authOptions;

        private readonly ITestOutputHelper _output;

        public IdentityServiceTests(ITestOutputHelper output)
        {
            _jwtService = new JwtService(_tokenLifeTimeOptions.Object, _tokenOptions.Object, _authOptions.Object);

            _identityService = new IdentityService(_userRepository.Object,
                _roleRepository.Object, _userRoleRepository.Object, _refreshTokenRepository.Object,
                _jwtService, _hashService, _tokenLifeTimeOptions.Object);

            _output = output;
        }
    }
}