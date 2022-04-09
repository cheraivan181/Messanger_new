namespace Core.Repositories.Interfaces;

public interface IConnectionRepository
{
    Task<Guid> AddConnectionAsync(Guid userId, Guid sessionId, string connectionId);
    Task RemoveConnectionAsync(string connectionId);
}