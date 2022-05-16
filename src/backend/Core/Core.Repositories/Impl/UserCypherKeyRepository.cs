using Core.DbModels;
using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;
using Dapper;

namespace Core.Repositories.Impl;

public class UserCypherKeyRepository : IUserCypherKeyRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public UserCypherKeyRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateAsync(Guid sessionId, string key)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        var result = Guid.NewGuid();

        string sql = "INSERT INTO UserCypherKeys (Id, CryptedKey, SessionId, CreatedAt) "
                     + "VALUES (@id, @cryptedKey, @sessionId, @createdAt)";

        await connection.ExecuteAsync(sql,
            new {id = result, cryptedKey = key, sessionId = sessionId, createdAt = DateTime.Now});

        return result;
    }

    public async Task<UserCypherKey> GetCypherKeyBySessionIdAsync(Guid sessionId)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        var result = await connection.QuerySingleOrDefaultAsync<UserCypherKey>(
            "SELECT * FROM UserCypherKeys WHERE SessionId = @sessionId", new
            {
                sessionId = sessionId
            });

        return result;
    }
}