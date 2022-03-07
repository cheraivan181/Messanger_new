using Core.DbModels;
using Core.DbModels.Base.Interface;
using Core.Repositories.Interfaces;
using Dapper;

namespace Core.Repositories.Impl
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public UserRoleRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> IsUserRoleExist(long userId, int roleId)
        {
            using var connection = await _connectionFactory.GetDbConnectionAsync();
            string sql = "SELECT COUNT(*) FROM UserRoles WHERE UserId = @userId and RoleId = @roleId";
            var result = await connection.ExecuteScalarAsync<int>(sql, new { userId = userId, roleId = roleId });

            return result > 0;
        } 

        public async Task AddUserRoleAsync(long userId, long roleId)
        {
            using var connection = await _connectionFactory.GetDbConnectionAsync();
            string sql = "INSERT INTO UserRoles (UserId, RoleId)" +
                "VALUES (@userId, @roleId)";

            await connection.ExecuteAsync(sql, new { userId = userId, roleId = roleId });
        }

        public async Task<List<Role>> GetUserRolesAsync(long userId)
        {
            using var connection = await _connectionFactory.GetDbConnectionAsync();
            string sql = "SELECT * FROM Roles r " +
                "INNER JOIN UserRoles u ON r.Id = u.RoleId AND u.UserId = @userId";

            var roles = await connection.QueryAsync<Role>(sql, new { userId = userId });
            return roles.ToList();
        }
    }
}
