namespace Core.Repositories.Interfaces;

public interface IConnectionRepository
{
    Task<long> AddConnectionAsync(long userId, long sessionId, string connectionId);
    Task RemoveConnectionAsync(string connectionId);
}