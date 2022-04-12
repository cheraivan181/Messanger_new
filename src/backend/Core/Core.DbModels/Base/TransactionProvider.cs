using System.Data.Common;
using System.Transactions;
using Core.DbModels.Base.Interface;

namespace Core.DbModels.Base;

public class TransactionProvider : ITransactionProvider
{
    private readonly IConnectionFactory _connectionFactory;

    public TransactionProvider(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<DbTransaction> GetDbTransactionAsync()
    {
        using var connection = await _connectionFactory.GetDbConnectionAsync(); 
        var transaction = await connection.BeginTransactionAsync();
        return transaction;
    }
}