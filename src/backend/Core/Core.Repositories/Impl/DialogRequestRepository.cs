using Core.DbModels;
using Core.DbModels.Base.Interface;

namespace Core.Repositories;

public class DialogRequestRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public DialogRequestRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> CreateDialogRequestAsync(long ownerUserId, long requestUserId)
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync();
        string sql = "";

        return 0;
    }
}