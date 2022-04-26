using Core.DbModels;
using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;

namespace Core.Repositories.Impl;

public class MessageRepository : IMessageRepository
{
    private readonly IConnectionFactory _connectionFactory;
    private const int PageList = 30;
    
    public MessageRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Message>> GetMessageFromDialogAsync(Guid dialogId)
    {
        var result = new List<Message>();
        using var db = _connectionFactory.GetDbConnectionAsync();

        return result;
    }
}