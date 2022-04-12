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

        public async Task<User> GetUserByIdAsync(Guid userId)
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

        public async Task<Guid> CreateUserAsync(string userName,
            string phone,
            string email,
            string password)
        {
              using var conn = await _connectionFactory.GetDbConnectionAsync();
              var result = Guid.NewGuid();
              
              string sql = "INSERT INTO Users (Id, Phone, UserName, Email, Password, CreatedAt) " +
                           "VALUES (@id, @phone, @userName, @email, @password, @createdAt);";
            
              await conn.ExecuteAsync(sql,
                    new
                    {
                        id = result,
                        phone = phone,
                        userName = userName,
                        email = email,
                        password = password,
                        createdAt = DateTime.Now
                    });
            
              return result;
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            using var conn = await _connectionFactory.GetDbConnectionAsync();
            var result = await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users u " +
                "INNER JOIN RefreshTokens r ON r.Value = @refreshToken", new { refreshToken  = refreshToken});

            return result;
        }

        public async Task<List<User>> SearchUsersByUserNameAsync(string userName, int page = 0)
        {
            const int countUsersInPage = 10;
            int skipUsers = page * countUsersInPage;
            
            using var connection = await _connectionFactory.GetDbConnectionAsync();
            
            string sql =
                $"SELECT TOP({skipUsers + countUsersInPage}) u.Id, u.UserName, COUNT(d.Id) as 'DialogCounts' FROM Users u "
                + "LEFT JOIN Dialogs d ON d.User1Id = u.Id or d.User2Id = u.Id "
                + "WHERE UserName LIKE @userName "
                + "GROUP BY u.Id, u.UserName "
                + "HAVING COUNT(d.Id) = 0";
            
            var result = (await connection.QueryAsync<User>(sql, new {userName = $"{userName}%"}))
                .ToList();
            
            return result;
        }
    }
}
