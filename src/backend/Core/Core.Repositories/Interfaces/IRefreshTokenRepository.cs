using Core.DbModels;

namespace Core.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<bool> CreateRefreshTokenAsync(long userId, string value);
        Task<List<string>> GetUserRefreshTokenValuesAsync(long userId);
        Task<RefreshToken> GetRefreshTokenByValueAsync(string value);
        Task<bool> UpdateRefreshTokenAsync(long userId, string oldValue, string newValue);
    }
}
