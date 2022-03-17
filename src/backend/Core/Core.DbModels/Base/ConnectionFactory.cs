using Core.DbModels.Base.Interface;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace Core.DbModels.Base
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;
        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DbConnection> GetDbConnectionAsync()
        {
            var connectionString = _configuration.GetConnectionString("MsSql");
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            return connection;
        }
    }
}
