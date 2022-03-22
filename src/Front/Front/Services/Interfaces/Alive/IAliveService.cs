namespace Front.Services.Interfaces.Alive
{
    public interface IAliveService
    {
        Task<bool> IsApiAliveAsync();
    }
}
