using Core.CacheServices.Interfaces.Base;
using Core.SessionServices.Services.Interfaces;

namespace Core.SessionServices.Services.Implementations;

public class SessionCacheService : ISessionCacheService
{
    private readonly IDatabaseProvider _databaseProvider;

    public SessionCacheService(IDatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider;
    }

    public async Task AddSessionInCacheAsync()
    {
    }

    public async Task GetSessionFromCacheAsync()
    {
    }
}