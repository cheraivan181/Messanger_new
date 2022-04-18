using Core.DbModels;

namespace Core.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserNameAndPassword(string userName, string hashPassword);
        Task<bool> CheckUserByUserNameAsync(string userName);
        Task<Guid> CreateUserAsync(string userName,
            string phone,
            string email,
            string password);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task<List<User>> SearchUsersByUserNameAsync(Guid userId, string userName, int page = 0);
    }
}
