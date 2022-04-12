using System.Data.Common;

namespace Core.DbModels.Base.Interface;

public interface ITransactionProvider
{
    Task<DbTransaction> GetDbTransactionAsync();
}