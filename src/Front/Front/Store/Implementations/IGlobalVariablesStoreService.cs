namespace Front.Store.Implementations
{
    public interface IGlobalVariablesStoreService
    {
        ValueTask SetIsAliveAsync(bool isAlive);
        ValueTask SetIsAuthenticated(bool isAuth);
        ValueTask SetIsGlobalError(bool isGlobalError);
        ValueTask<bool> GetIsAliveAsync();
        Task<bool> GetIsAuthenticated();
        Task<bool> GetIsGlobalError();
    }
}
