using Core.DbModels;
using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;
using Dapper;

namespace Core.Repositories.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public UserRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User> GetUserByIdAsync(long userId)
        {
            using var conn = await _connectionFactory.GetDbConnectionAsync();
            return 
                await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @userId", new { userId = userId });
        }

        public async Task<User> GetUserByUserNameAndPassword(string userName, string hashPassword)
        {
            using (var conn = await _connectionFactory.GetDbConnectionAsync())
            {
                string sql = "SELECT * FROM Users WHERE [UserName] = @userName and [Password] = @hashPassword";

                var user = await conn.QueryFirstOrDefaultAsync<User>(sql, new
                {
                    userName = userName,
                    hashPassword = hashPassword
                });

                return user;
            }
        }

        public async Task<bool> CheckUserByUserNameAsync(string userName)
        {
            using var conn = await _connectionFactory.GetDbConnectionAsync();
            string sql = "SELECT COUNT(*) FROM [Users] WHERE [UserName] = @userName";
            var result = await conn.ExecuteScalarAsync<int>(sql, new { userName = userName });

            return result == 1;
        }

        public async Task<long> CreateUserAsync(string userName,
            string phone,
            string email,
            string password)
        {
            using var conn = await _connectionFactory.GetDbConnectionAsync();

            string sql = "INSERT INTO Users (Phone, UserName, Email, Password, CreatedAt) " +
                "VALUES (@phone, @userName, @email, @password, @createdAt); " +
                "SELECT SCOPE_IDENTITY();";

            var userId = await conn.ExecuteScalarAsync<long>(sql, 
                new { phone = phone, userName = userName, email = email, password = password, createdAt = DateTime.Now });
            return userId;
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            using var conn = await _connectionFactory.GetDbConnectionAsync();
            var result = await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users u " +
                "INNER JOIN RefreshTokens r ON r.Value = @refreshToken", new { refreshToken  = refreshToken});

            return result;
        }
    }
}
