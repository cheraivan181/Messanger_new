using Core.DbModels;
using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;
using Dapper;

namespace Core.Repositories.Impl
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public RoleRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            using var connection = await _connectionFactory.GetDbConnectionAsync();
            string sql = "SELECT * FROM Roles WHERE Name = @roleName";
            var role = await connection.QueryFirstOrDefaultAsync<Role>(sql, new { roleName = roleName });

            return role;
        }
    }
}
