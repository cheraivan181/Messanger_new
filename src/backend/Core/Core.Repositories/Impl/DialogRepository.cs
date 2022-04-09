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

    public async Task<(List<Dialog> dialogs, List<DialogRequest> dialogRequest)>
        GetUserDialogsAndDialogRequestsAsync(Guid userId, string predicate)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        var queryParams = new
        {
            predicate = $"{predicate}%",
            userId = userId
        };

        string sql = "SELECT d.Id, u.Id, d.User1Id, d.User2Id, u.UserName FROM Dialogs d WITH(NOLOCK) "
                     + "INNER JOIN Users u WITH(NOLOCK) ON d.User1Id = u.Id or d.User2Id = u.Id "
                     + "WHERE (d.User1Id = @userId OR d.User2Id = @userId) and u.UserName LIKE @predicate;";


        var dialogs = (await connection.QueryAsync<Dialog, User, Dialog>(sql, (dialog, user) =>
        {
            if (dialog.User1Id == userId)
                dialog.User2 = user;
            else
                dialog.User1 = user;

            return dialog;
        }, queryParams, splitOn: "Id")).ToList();

        sql = "SELECT d.Id, u.Id, d.OwnerUserId, u.UserName FROM DialogRequests d WITH(NOLOCK) "
              + "INNER JOIN Users u WITH(NOLOCK) ON d.RequestUserId = u.Id "
              + "WHERE d.OwnerUserId = @userId and u.UserName LIKE @predicate";

        var dialogRequests = (await connection.QueryAsync<DialogRequest, User, DialogRequest>(sql,
            (dialogRequest, user) =>
            {
                dialogRequest.RequestUser = user;
                return dialogRequest;
            }, queryParams, splitOn: "Id")).ToList();

        return (dialogs, dialogRequests);
    }
}