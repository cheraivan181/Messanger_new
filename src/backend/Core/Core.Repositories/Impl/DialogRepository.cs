using Core.DbModels;
using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;
using Dapper;

namespace Core.Repositories.Impl;

public class DialogRepository : IDialogRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public DialogRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<Guid> CreateDialogAsync(Guid user1Id, Guid user2Id, Guid dialogRequestId)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        var result = Guid.NewGuid();

        string sql = "INSERT INTO Dialogs (Id, User1Id, User2Id, Created, DialogRequestId) " +
                     "VALUES (@id, @user1id, @user2id, @created, @dialogRequestId)";

        await connection.ExecuteAsync(sql, new
        {
            id = result,
            user1id = user1Id,
            user2id = user2Id,
            created = DateTime.Now,
            dialogRequestId = dialogRequestId
        });
        
        return result;
    }

    public async Task<List<Dialog>> GetUserDialogsAsync(Guid userId)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        var test = await connection.QuerySingleOrDefaultAsync<Dialog>("SELECT TOP 1 * FROM Dialogs");
        
        string sql =
            "SELECT * FROM Dialogs d "
            + "INNER JOIN Users u ON u.Id = d.User1Id OR u.Id = d.User2Id "
            + "INNER JOIN DialogRequests dr ON dr.Id = d.DialogRequestId "
            + "WHERE User1Id = @userId OR User2Id = @userId";

        var result = await connection.QueryAsync<Dialog, User, DialogRequest, Dialog>(sql,
            ((dialog, user, dialogRequest) =>
            {
                dialog.DialogRequest = dialogRequest;
                if (dialog.User1Id == userId)
                    dialog.User2 = user;
                else
                    dialog.User1 = user;

                return dialog;
            }), new {userId = userId}, splitOn: "Id, Id");

        return result.ToList();
    }

    public async Task<Dialog> GetDialogAsync(Guid dialogId)
    {
        using var db = await _connectionFactory.GetDbConnectionAsync();
        var result = await db.QueryFirstOrDefaultAsync<Dialog>("SELECT * FROM Dialogs WHERE Id = @id",
            new {id = dialogId});
        return result;
    }
}