using Core.DbModels;

namespace Core.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByNameAsync(string roleName);
    }
}
