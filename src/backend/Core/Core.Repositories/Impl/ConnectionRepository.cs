using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;
using Dapper;

namespace Core.Repositories.Impl
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        
        public ConnectionRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        
        public async Task<long> AddConnectionAsync(long userId, long sessionId, string connectionId)
        {
            using var connection = await _connectionFactory.GetDbConnectionAsync();
            var sql = "INSERT INTO Connections(UserId, SessionId, Value, CreatedAt) "
                      + "VALUES (@userId, @sessionId, @connectionId, @createdAt); "
                      + "SELECT SCOPE_IDENTITY()";
            
            var result = await connection.ExecuteScalarAsync<long>(sql, new
            {
                userId = userId,
                sessionId = sessionId,
                connectionId = connectionId,
                createdAt = DateTime.Now
            });

            return result;
        }

        public async Task RemoveConnectionAsync(string connectionId)
        {
            using var connection = await _connectionFactory.GetDbConnectionAsync();
            var sql = "DELETE FROM Connections "
                      + "WHERE [Value] = @connectionId";
            await connection.ExecuteAsync(sql, new {connectionId = connectionId});
        }
    }
}
