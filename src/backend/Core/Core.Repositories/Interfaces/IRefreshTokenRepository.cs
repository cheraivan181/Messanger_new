using Core.DbModels;

namespace Core.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<bool> CreateRefreshTokenAsync(Guid userId, string value);
        Task<List<string>> GetUserRefreshTokenValuesAsync(Guid userId);
        Task<RefreshToken> GetRefreshTokenByValueAsync(string value);
        Task<bool> UpdateRefreshTokenAsync(Guid userId, string oldValue, string newValue);
    }
}
