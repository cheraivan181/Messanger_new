using Core.DbModels;

namespace Core.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<bool> IsUserRoleExist(Guid userId, int roleId);
        Task AddUserRoleAsync(Guid userId, int roleId);
        Task<List<Role>> GetUserRolesAsync(Guid userId);
    }
}
