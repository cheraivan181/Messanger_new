using Core.DbModels;

namespace Core.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<bool> IsUserRoleExist(long userId, int roleId);
        Task AddUserRoleAsync(long userId, int roleId);
        Task<List<Role>> GetUserRolesAsync(long userId);
    }
}
