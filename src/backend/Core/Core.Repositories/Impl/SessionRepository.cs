using Core.DbModels;
using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;
using Dapper;

namespace Core.Repositories.Impl;

public class SessionRepository : ISessionRepository
{
    private readonly IConnectionFactory _connectionFactory;
    
    public SessionRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateSessionAsync(Guid userId, string serverPrivateKey, 
        string serverPublicKey, string clientPublicKey)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        var result = Guid.NewGuid();
        string sql = "INSERT INTO Sessions (Id, UserId, ClientPublicKey, ServerPublicKey, ServerPrivateKey, CreatedAt) "
                     + "VALUES (@id, @userId, @clientPublicKey, @serverPublicKey, @serverPrivateKey, @createdAt);";
        
        await connection.ExecuteAsync(sql, new
        {
            id = result,
            userId = userId,
            clientPublicKey = clientPublicKey,
            serverPublicKey = serverPublicKey,
            serverPrivateKey = serverPrivateKey,
            createdAt = DateTime.Now
        });

        return result;
    }

    public async Task<Session> GetSessionAsync(Guid sessionId)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        string sql = "SELECT * FROM Sessions WHERE Id = @sessionId";
        var result = await connection.QueryFirstAsync<Session>(sql, new {sessionId = sessionId});
        return result;
    }
}