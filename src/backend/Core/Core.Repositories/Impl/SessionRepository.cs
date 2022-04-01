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

    public async Task<long> CreateSessionAsync(long userId, string serverPrivateKey, 
        string serverPublicKey, string clientPublicKey)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        string sql = "INSERT INTO Sessions (UserId, ClientPublicKey, ServerPublicKey, ServerPrivateKey) " 
            + "VALUES (@userId, @clientPublicKey, @serverPublicKey, @serverPrivateKey); "
            + "SELECT SCOPE_IDENTITY();";
        
        var result = await connection.ExecuteScalarAsync<long>(sql, new
        {
            userId = userId,
            clientPublicKey = clientPublicKey,
            serverPublicKey = serverPublicKey,
            serverPrivateKey = serverPrivateKey
        });

        return result;
    }

    public async Task<Session> GetSessionAsync(long sessionId)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        string sql = "SELECT * FROM Sessions WHERE Id = @sessionId";
        var result = await connection.QueryFirstAsync<Session>(sql, new {Id = sessionId});
        return result;
    }
}