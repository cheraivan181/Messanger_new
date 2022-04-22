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

    public async Task<Guid> CreateDialogSecretAsync(Guid dialogId, string key)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        var result = Guid.NewGuid();
        string sql = "INSERT INTO DialogSecrets (Id, DialogId, CypherKey, CreatedAt) " +
                     "VALUES (@id, @dialogId, @cypherKey, @createdAt)";
        
        await connection.ExecuteAsync(sql, new
        {
            id = result,
            dialogId = dialogId,
            cypherKey = key,
            createdAt = DateTime.Now
        });

        return result;
    }

    public async Task<Dictionary<Guid, string>> GetDialogSecretsAsync(IEnumerable<Guid> dialogIds)
    {
        var result = new Dictionary<Guid, string>();
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        string sql = "SELECT * FROM DialogSecrets WHERE DialogId IN @dialogIds";
        var queryResult = await connection.QueryAsync<DialogSecret>(sql, new {dialogIds = dialogIds});

        foreach (var dialog in queryResult.AsEnumerable())
            result.Add(dialog.Id, dialog.CypherKey);
        
        return result;
    }
}