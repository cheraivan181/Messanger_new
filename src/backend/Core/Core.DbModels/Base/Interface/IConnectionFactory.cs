using System.Data.Common;

namespace Core.DbModels.Base.Interface
{
    public interface IConnectionFactory
    {
        Task<DbConnection> GetDbConnectionAsync();
    }
}
