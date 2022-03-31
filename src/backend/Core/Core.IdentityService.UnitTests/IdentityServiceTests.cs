using Core.CryptService.Impl;
using Core.CryptService.Interfaces;
using Core.DbModels;
using Core.IdentityService.Domain.Options;
using Core.IdentityService.Interfaces;
using Core.IdentityService.Services;
using Core.IdentityService.UnitTests.Models;
using Core.Repositories.Interfaces;
using Core.Utils;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Core.IdentityService.UnitTests
{
    [Trait("Category", "IdentityServiceTests")]
    public class IdentityServiceTests
    { 
        private readonly IIdentityService _identityService;
        private readonly IHashService _hashService;

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IUserRoleRepository> _userRoleRepositoryMock;

        private readonly IJwtService _jwtService;

        private readonly IOptions<TokenLifeTimeOptions> _tokenLifeTimeOptions;
        private readonly IOptions<TokenOptions> _tokenOptions;
        private readonly IOptions<AuthOptions> _authOptions;

        private readonly ITestOutputHelper _output;

        private List<User> _users = new List<User>();
        private List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        private List<Role> _roles = new List<Role>();
        private List<UserRoles> _userRoles = new List<UserRoles>();

        private static object lockObject = new object();

        public IdentityServiceTests(ITestOutputHelper output)
        {
            _output = output;
            _userRepositoryMock = new Mock<IUserRepository>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _userRoleRepositoryMock = new Mock<IUserRoleRepository>();
            _tokenLifeTimeOptions = Options.Create(new TokenLifeTimeOptions()
            {
                AccessTokenLifeTime = 60,
                RefreshTokenLifeTime = 1000
            });
            _tokenOptions = Options.Create(new TokenOptions()
            {
                IsJwe = false,
                Key = "k72gnxq3pkum9toiub48o8s8sdbjhme1tg0m3p4jfkzovsgdqzgv6t47ig3tr5d9",
                CypherKey = "1tg0m3p4jfkzovsgk72gnxq3pkum9toiub48o8s8sdbjhmedqzgv6t47ig3tr5d9"
            });
            _authOptions = Options.Create(new AuthOptions()
            {
                Issuer = "MessangerApi",
                Audience = "http://localhost:3000,https://localhost:3000"
            });

            _hashService = new HashService();
            _jwtService = new JwtService(_tokenLifeTimeOptions, _tokenOptions, _authOptions);
            _identityService = new IdentityService(_userRepositoryMock.Object, _roleRepositoryMock.Object,
                _userRoleRepositoryMock.Object, _refreshTokenRepositoryMock.Object, _jwtService, _hashService,
                _tokenLifeTimeOptions);

            SetMocks();
            InitTestData();
        }


        [Theory]
        [MemberData(nameof(TestCases.GetSignInTestData), MemberType = typeof(TestCases))]
        public async Task TestSignInAsync(SignInTestData testData)
        {
            var result = await _identityService.SignInAsync(testData.UserName, testData.Password);

            if (testData.IsSucessOperation)
            {
                result.AcessToken.Should().NotBeNullOrEmpty();
                result.RefreshToken.Should().NotBeNullOrEmpty();
                result.IsSucess.Should().BeTrue();
                result.IsUnAuthorizedError.Should().BeFalse();
                result.ErrorMessage.Should().BeNullOrEmpty();
                _refreshTokens.Any(x => x.Value == result.RefreshToken)
                    .Should().BeTrue();

                return;
            }
          
            result.AcessToken.Should().BeNullOrEmpty();
            result.RefreshToken.Should().BeNullOrEmpty();
            result.ErrorMessage.Should().NotBeNullOrEmpty();
        }
        
        [Theory]
        [MemberData(nameof(TestCases.GetSignUpTestData), MemberType = typeof(TestCases))]
        public async Task TestSignUpAsync(SignUpTestData testData)
        {
            var result = await _identityService.SignUpAsync(testData.UserName, testData.Phone, testData.Email, testData.Password);

            result.AcessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
            result.IsSucess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();

            var user = _users.FirstOrDefault(x => x.UserName == testData.UserName && x.Email == testData.Email);
            user.Should().NotBeNull();

            _refreshTokens.Any(x => x.Value == result.RefreshToken)
                .Should().BeTrue();

            var userRoles = _userRoles.Where(x => x.UserId == user.Id).ToList();
            userRoles.Count.Should().BeGreaterThan(0);
        }

        [Theory]
        [MemberData(nameof(TestCases.GetUpdateRefreshTokenTestData), MemberType = typeof(TestCases))]
        public async Task TestUpdateRefreshTokenAsync(UpdateRefreshTokenTestData testData)
        {
            var result = await _identityService.UpdateJwtAsync(testData.RefreshToken);
            if (testData.IsSucess)
            {
                result.AcessToken.Should().NotBeNullOrEmpty();
                result.RefreshToken.Should().NotBeNullOrEmpty();
                result.IsSucess.Should().BeTrue();

                _refreshTokens.Any(x => x.Value == testData.RefreshToken)
                    .Should().BeFalse();
                _refreshTokens.Any(x => x.Value == result.RefreshToken)
                    .Should().BeTrue();
            }
            else
            {
                result.AcessToken.Should().BeNullOrEmpty();
                result.RefreshToken.Should().BeNullOrEmpty();
                result.IsSucess.Should().BeFalse();
                result.ErrorMessage.Should().NotBeNullOrEmpty();
            }
        }

        public void SetMocks()
        {
            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<long>()))
                .Returns((long id) => Task.FromResult(_users.FirstOrDefault(x => x.Id == id)));
            _userRepositoryMock.Setup(x => x.GetUserByUserNameAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string userName, string hashPassword) =>
                {
                    return Task.FromResult(_users.FirstOrDefault(x => x.UserName == userName && x.Password == hashPassword));
                }); 

            _userRepositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string userName, string phone, string email, string hashPassword) =>
                {
                    var user = _users.FirstOrDefault(x => x.UserName == userName && x.Email == email);
                    if (user != null)
                        return;

                    lock (lockObject)
                    {
                        var maxId = _users.Max(x => x.Id) + 1;    
                        _users.Add(new User()
                        {
                            Id = maxId,
                            UserName = userName,
                            Phone = phone,
                            Email = email,
                            Password = hashPassword
                        });
                    }
                })
                .Returns((string userName, string phone, string email, string hashPassword) =>
                {
                    var user = _users.Single(x => x.UserName == userName && x.Email == email);
                    return Task.FromResult(user.Id);
                });

            _userRepositoryMock.Setup(x => x.CheckUserByUserNameAsync(It.IsAny<string>()))
                .Returns((string userName) => Task.FromResult(_users.Any(x => x.UserName == userName)));

            _roleRepositoryMock.Setup(x => x.GetRoleByNameAsync(It.IsAny<string>()))
                .Returns((string roleName) => Task.FromResult(_roles.FirstOrDefault(role => role.Name == roleName)));

            _userRoleRepositoryMock.Setup(x => x.AddUserRoleAsync(It.IsAny<long>(), It.IsAny<int>()))
                .Callback((long userId, int roleId) =>
                {
                    var userRole = _userRoles.FirstOrDefault(x => x.UserId == userId && x.RoleId == roleId);
                    if (userRole != null)
                        return;

                    lock (lockObject)
                    {
                        var maxId = _userRoles.Max(x => x.Id) + 1;
                        _userRoles.Add(new UserRoles()
                        {
                            Id = maxId,
                            RoleId = roleId,
                            UserId = userId
                        });
                    }
                }).Returns(() => Task.CompletedTask);

            _userRoleRepositoryMock.Setup(x => x.GetUserRolesAsync(It.IsAny<long>()))
                .Returns((long userId) =>
                {
                    var roles = _userRoles.Where(x => x.UserId == userId)
                        .Join(_roles,
                            userRole => userRole.RoleId,
                            role => role.Id,
                            (userRole, role) => role)
                        .ToList();

                    return Task.FromResult(roles);
                });

            _refreshTokenRepositoryMock.Setup(x => x.CreateRefreshTokenAsync(It.IsAny<long>(), It.IsAny<string>()))
                .Returns((long userId, string refreshToken) =>
                {
                    lock (lockObject)
                    {
                        var maxId = _refreshTokens.Max(x => x.Id) + 1;
                        _refreshTokens.Add(new RefreshToken()
                        {
                            Id = maxId,
                            UserID = userId,
                            CreatedAt = DateTime.Now,
                            Value = refreshToken
                        });
                    }

                    return Task.FromResult(true);
                });

            _refreshTokenRepositoryMock.Setup(x => x.GetRefreshTokenByValueAsync(It.IsAny<string>()))
                .Returns((string refreshToken) =>
                {
                    var currentRefreshToken = new RefreshToken();
                    currentRefreshToken = _refreshTokens.FirstOrDefault(x => x.Value == refreshToken);
                    if (currentRefreshToken == null)
                        return Task.FromResult(currentRefreshToken);

                    var user = _users.FirstOrDefault(x => x.Id == currentRefreshToken.UserID);
                    currentRefreshToken.User = user;
                    return Task.FromResult(currentRefreshToken);
                });

            _refreshTokenRepositoryMock.Setup(x => x.UpdateRefreshTokenAsync(It.IsAny<long>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns((long userId, string oldValue, string newValue) =>
                {
                    var refreshToken = _refreshTokens.FirstOrDefault(x => x.UserID == userId && x.Value == oldValue);
                    if (refreshToken == null)
                        return Task.FromResult(false);

                    refreshToken.Value = newValue;
                    return Task.FromResult(true);
                });
        }

        private void InitTestData()
        {
            _roles = new List<Role>()
            {
                new Role(){Id = 0, Name = "User"},
                new Role() {Id = 1, Name = "Admin"},
                new Role() {Id = 2, Name = "ProtocoledUsers"}
            };
            _users = new List<User>()
            {
                new User()
                {
                    Id = 0,
                    CreatedAt = DateTime.Now,
                    Email = "test1@gmail.com",
                    Password = _hashService.GetHash("test"),
                    Phone = "+38012345678",
                    UserName = "testuser1"
                },
                new User()
                {
                    Id = 1,
                    CreatedAt = DateTime.Now,
                    Email = "test2@gmail.com",
                    Password = _hashService.GetHash("test2"),
                    Phone = "+380123456782",
                    UserName = "testuser2"
                },
                new User()
                {
                    Id = 2,
                    CreatedAt = DateTime.Now,
                    Email = "test3@gmail.com",
                    Password = _hashService.GetHash("test3"),
                    Phone = "+380123456783",
                    UserName = "testuser13"
                },
                new User()
                {
                    Id = 3,
                    CreatedAt = DateTime.Now,
                    Email = "test4@gmail.com",
                    Password = _hashService.GetHash("test4"),
                    Phone = "+380123456784",
                    UserName = "testuser14"
                },
                new User()
                {
                    Id = 4,
                    CreatedAt = DateTime.Now,
                    Email = "test5@gmail.com",
                    Password = _hashService.GetHash("test"),
                    Phone = "+380152345678",
                    UserName = "testuser15"
                }
            };
            _refreshTokens = new List<RefreshToken>()
            {
                new RefreshToken()
                {
                    Id = 0,
                    UserID = 0,
                    CreatedAt = DateTime.Now,
                    Value = "12345678910111234123"
                },
                new RefreshToken()
                {
                    Id = 1,
                    UserID = 1,
                    CreatedAt = DateTime.Now,
                    Value = "123456789101112341231"
                },
                new RefreshToken()
                {
                    Id = 2,
                    UserID = 2,
                    CreatedAt = DateTime.Now,
                    Value = "1234567891011123412311"
                },
                new RefreshToken()
                {
                    Id = 3,
                    UserID = 3,
                    CreatedAt = DateTime.Now,
                    Value = "123456789101112341231132"
                }
            };
            _userRoles = new List<UserRoles>()
            {
                new UserRoles()
                {
                    Id = 0,
                    UserId = 0,
                    RoleId = 0
                },
                new UserRoles()
                {
                    Id = 1,
                    UserId = 1,
                    RoleId = 0
                },
                new UserRoles()
                {
                    Id = 1,
                    UserId = 2,
                    RoleId = 0
                },
                new UserRoles()
                {
                    Id = 1,
                    UserId = 3,
                    RoleId = 0
                },
                new UserRoles()
                {
                    Id = 1,
                    UserId = 4,
                    RoleId = 0
                }
            };
        }
    }
}