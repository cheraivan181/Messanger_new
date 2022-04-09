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
        
        public async Task<Guid> AddConnectionAsync(Guid userId, Guid sessionId, string connectionId)
        {
            using var connection = await _connectionFactory.GetDbConnectionAsync();
            var result = Guid.NewGuid();
            var sql = "INSERT INTO Connections(Id, UserId, SessionId, Value, CreatedAt) "
                      + "VALUES (@id, @userId, @sessionId, @connectionId, @createdAt);";
            
            await connection.ExecuteAsync(sql, new
            {
                id = result,
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
