using Core.DbModels;
using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Core.Repositories;

public class DialogRequestRepository : IDialogRequestRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public DialogRequestRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateDialogRequestAsync(Guid ownerUserId, Guid requestUserId)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        var result = Guid.NewGuid();
        string sql = "INSERT INTO DialogRequests (Id, OwnerUserId, RequestUserId, IsAccepted, CreatedAt) " + 
                     "VALUES (@id, @ownerUserId, @requestUserId, @isAccepted, @createdAt);";

        await connection.ExecuteAsync(sql, new
        {
            id = result,
            ownerUserId = ownerUserId,
            requestUserId = requestUserId,
            isAccepted = false,
            createdAt = DateTime.Now
        });
        
        return result;
    }

    public async Task ProcessDialogRequestAsync(Guid ownerUserId, Guid requestUserId, bool isAccepted)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        string sql = "UPDATE DialogRequests " +
                     "SET IsProcessed = 1, IsAccepted = @isAccepted " +
                     "WHERE OwnerUserId = @ownerUserId AND RequestUserId = @requestUserId";
        
        await connection.ExecuteAsync(sql, new
        {
            isAccepted = isAccepted ? 1 : 0,
            owneruserId = ownerUserId,
            requestUserId = requestUserId
        });
    }
    
    public async Task<DialogRequest> GetDialogRequestAsync(Guid ownerUserId, Guid requestUserId)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        string sql = "SELECT * FROM DialogRequests WHERE OwnerUserId = @ownerUserId and RequestUserId = @requestUserId";
        var result =
            await connection.QueryFirstOrDefaultAsync<DialogRequest>(sql,
                new {ownerUserId = ownerUserId, requestUserId = requestUserId});
        return result;
    }
}