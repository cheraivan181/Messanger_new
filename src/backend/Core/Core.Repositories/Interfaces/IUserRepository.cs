using Core.DbModels;

namespace Core.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserNameAndPassword(string userName, string hashPassword);
        Task<bool> CheckUserByUserNameAsync(string userName);
        Task<long> CreateUserAsync(string userName,
            string phone,
            string email,
            string password);
        Task<User> GetUserByIdAsync(long userId);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task<List<User>> SearchUsersByUserNameAsync(string userName, int page = 0);
    }
}
