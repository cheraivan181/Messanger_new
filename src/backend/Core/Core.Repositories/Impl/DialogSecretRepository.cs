using Core.DbModels;
using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;
using Dapper;

namespace Core.Repositories.Impl;

public class DialogSecretRepository : IDialogSecretRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public DialogSecretRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateDialogSecretAsync(Guid dialogId, string key, string iv)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        var result = Guid.NewGuid();
        string sql = "INERT INTO DialogSecrets (Id, DialogId, CypherKey, CreatedAt, IV) " +
                     "VALUES (@id, @dialogId, @cypherKey, @createdAt, @iv)";
        
        await connection.ExecuteAsync(sql, new
        {
            id = result,
            dialogId = dialogId,
            cypherKey = key,
            iv = iv
        });

        return result;
    }

    public async Task<Dictionary<Guid, (string key, string iv)>> GetDialogSecretsAsync(IEnumerable<Guid> dialogIds)
    {
        var result = new Dictionary<Guid, (string key, string iv)>();
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        string sql = "SELECT * FROM DialogSecrets WHERE DialogId IN @dialogIds";
        var queryResult = await connection.QueryAsync<DialogSecret>(sql, new {dialogIds = dialogIds});

        foreach (var dialog in queryResult.AsEnumerable())
            result.Add(dialog.Id, (key: dialog.CypherKey, iv: dialog.IV));
        return result;
    }
}