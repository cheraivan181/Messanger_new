using Core.DbModels.Base.Interface;
using Microsoft.Extensions.Configuration;
using System.Data;
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
            var connection = new SqlConnection(_configuration.GetConnectionString("MsSql"));
            await connection.OpenAsync();

            return connection;
        }
    }
}
