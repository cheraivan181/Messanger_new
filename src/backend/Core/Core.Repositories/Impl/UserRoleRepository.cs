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

        public async Task<bool> IsUserRoleExist(Guid userId, int roleId)
        {
            using var connection = await _connectionFactory.GetDbConnectionAsync();
            string sql = "SELECT COUNT(*) FROM UserRoles WHERE UserId = @userId and RoleId = @roleId";
            var result = await connection.ExecuteScalarAsync<int>(sql, new { userId = userId, roleId = roleId });

            return result > 0;
        } 

        public async Task AddUserRoleAsync(Guid userId, int roleId)
        {
            using var connection = await _connectionFactory.GetDbConnectionAsync();
            string sql = "INSERT INTO UserRoles (Id, UserId, RoleId) " +
                         "VALUES (@id, @userId, @roleId)";

            await connection.ExecuteAsync(sql, new
            {
                id = Guid.NewGuid(),
                userId = userId, 
                roleId = roleId,
                createdAt = DateTime.Now
            });
        }

        public async Task<List<Role>> GetUserRolesAsync(Guid userId)
        {
            using var connection = await _connectionFactory.GetDbConnectionAsync();
            
            string sql = "SELECT r.Id, r.Name FROM Roles r " +
                         "INNER JOIN UserRoles u ON r.Id = u.RoleId " +
                         "WHERE u.UserId = @userId";

            var roles = await connection.QueryAsync<Role>(sql, new {userId = userId});
            return roles.ToList();
        }
    }
}
