using Core.DbModels;
using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;
using Dapper;

namespace Core.Repositories.Impl;

public class MessageRepository : IMessageRepository
{
    private readonly IConnectionFactory _connectionFactory;
    
    private const int CountMessagesInPage = 1000;
    private const int CountMessagesInCache = 20;
    
    public MessageRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Message>> GetMessageFromDialogAsync(Guid dialogId, int page = 0)
    {
        using var db = await _connectionFactory.GetDbConnectionAsync();
        int skipUser = page * CountMessagesInPage;

        string sql = $"SELECT TOP {CountMessagesInPage + skipUser} * FROM Messages " +
                     "WHERE DialogId = @dialogId";

        var result = await db.QueryAsync<Message>(sql, new {dialogId = dialogId});

        return result.Skip(CountMessagesInPage * page)
            .Take(CountMessagesInPage)
            .ToList();
    }

    public async Task<List<Message>> GetFirstMessagesFromDialogsAsync(Guid userId)
    {
        using var db = await _connectionFactory.GetDbConnectionAsync();
        string sql = "SELECT * FROM Dialogs d "
                     + "CROSS APPLY (SELECT TOP 1 * FROM Messages m WHERE m.DialogId = d.Id ORDER BY m.CreatedAt DESC) "
                     + "WHERE d.User1Id = @userId OR d.User2Id = @userId "
                     + "ORDER BY d.CreatedAt DESC";

        var messages = await db.QueryAsync<Message>(sql, new {userId = userId});
        return messages.ToList();
    }

    public async Task AddMessageAsync(Guid messageId, string cryptedText, Guid dialogId)
    {
        using var db = await _connectionFactory.GetDbConnectionAsync();
        string sql = "INSERT INTO Messages (Id, CryptedText, IsReaded, IsDeleted, DialogId, CreatedAt) "
            + "VALUES (@id, @cryptedText, @isReaded, @isDeleted, @dialogId, @createdAt)";

        await db.ExecuteAsync(sql, new
        {
            id = messageId,
            cryptedText = cryptedText,
            isReaded = false,
            isDeleted = false,
            dialogId = dialogId,
            createdAt = DateTime.Now
        });
        
    }

    public async Task<Dictionary<Guid, List<Message>>> GetMessageListsAsync(Guid userId)
    {
        using var db = await _connectionFactory.GetDbConnectionAsync();
        string sql = "SELECT * FROM Dialogs d "
                     + $"CROSS APPLY (SELECT TOP {CountMessagesInCache} * FROM Messages m WHERE m.DialogId = d.Id ORDER BY m.CreatedAt DESC) m "
                     + "WHERE d.User1Id = @userId OR d.User2Id = @userId "
                     + "ORDER BY d.CreatedAt DESC";
        
        var messages = await db.QueryAsync<Message>(sql, new {userId = userId});
        var dialogIds = messages.Select(x => x.DialogId)
            .Distinct();

        var result = new Dictionary<Guid, List<Message>>();
        
        foreach (var dialogId in dialogIds)
        {
            var dialogMessages = messages.Where(x => x.DialogId == dialogId)
                .ToList();
            result.Add(dialogId, dialogMessages);
        }

        return result;
    }
    
    public async Task SetMessageReadAsync(Guid messageId)
    {
        using var db = await _connectionFactory.GetDbConnectionAsync();
        string sql = "UPDATE Messages SET IsReaded = 1 "
                     + "WHERE Id = @messageId";
        
        await db.ExecuteAsync(sql, new {messageId = messageId});
    }

    public async Task UpdateTextMessageAsync(Guid messageId, string newText)
    {
        using var db = await _connectionFactory.GetDbConnectionAsync();
        string sql = "UPDATE Messages SET CryptedText = @text, UpdatedAt = @date "
                     + "WHERE Id = @messageId";

        await db.ExecuteAsync(sql, new {text = newText, messageId = messageId});
    }
    
    public async Task SetMessageDeleteAsync(Guid messageId)
    {
        using var db = await _connectionFactory.GetDbConnectionAsync();
        string sql = "UPDATE Messages SET IsDeleted = 1 "
                     + "WHERE Id = @messageId";

        await db.ExecuteAsync(sql, new {messageId = messageId});
    }

    public async Task<bool> IsMessageExistAsync(Guid messageId)
    {
        using var db = await _connectionFactory.GetDbConnectionAsync();
        string sql = "SELECT 1 FROM Messages WHERE Id = @messageId";
        var result = await db.ExecuteScalarAsync<int>(sql, new {messageId = messageId});

        return result == 1;
    }

    public async Task<Message> GetMessageAsync(Guid messageId)
    {
        using var db = await _connectionFactory.GetDbConnectionAsync();
        var result = await db.QueryFirstOrDefaultAsync<Message>("SELECT * FROM Messages WHERE Id = @id",
            new {id = messageId});
        return result;
    }
}