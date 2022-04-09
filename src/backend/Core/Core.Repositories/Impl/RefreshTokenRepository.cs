using Core.DbModels;
using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;
using Dapper;

namespace Core.Repositories.Impl
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public RefreshTokenRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<RefreshToken> GetRefreshTokenByValueAsync(string value)
        {
            using var conn = await _connectionFactory.GetDbConnectionAsync();
            string sql = "SELECT TOP 1 * FROM RefreshTokens r " +
                "INNER JOIN Users u ON r.UserId = u.Id " +
                "WHERE r.[Value] = @value";
            var refreshToken = await conn.QueryAsync<RefreshToken, User, RefreshToken>(sql, 
                (refreshToken, user) =>
                {
                    refreshToken.User = user;
                    return refreshToken;
                }, new { value  = value }, splitOn: "Id");

            return refreshToken?.FirstOrDefault();
        }

        public async Task<bool> CreateRefreshTokenAsync(Guid userId, string value)
        {
            await using var conn = await _connectionFactory.GetDbConnectionAsync();
            
            string sql = "INSERT INTO RefreshTokens (Id, UserId, Value, CreatedAt) " +
                "VALUES (@id, @userId, @value, @createdAt)";

            var result = await conn.ExecuteAsync(sql, 
                new
                {
                    id = Guid.NewGuid(),
                    userId = userId, 
                    value = value,
                    createdAt = DateTime.Now
                });
            return result > 0;
        }

        public async Task<List<string>> GetUserRefreshTokenValuesAsync(Guid userId)
        {
            using var conn = await _connectionFactory.GetDbConnectionAsync();
            var sql = "SELECT Value FROM RefreshTokens WHERE UserId = @userId";
            var result = await conn.QueryAsync<string>(sql, new { userId = userId });
            return result.ToList();
        }

        public async Task<bool> UpdateRefreshTokenAsync(Guid userId, string oldValue, string newValue)
        {
            using var conn = await _connectionFactory.GetDbConnectionAsync();
            string sql = "UPDATE RefreshTokens " +
                "SET [Value] = @newValue " +
                "WHERE UserId = @userId and [Value] = @oldValue";

            var result = await conn.ExecuteAsync(sql, 
                new { oldValue = oldValue, newValue = newValue, userId = userId });
            return result == 1;
        }
    }
}
