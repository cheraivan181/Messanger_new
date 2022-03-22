using Front.Domain.Responses;

namespace Front.Servives.Interfaces.Auth
{
    public interface IAuthOptionsService
    {
        ValueTask<AuthOptions> GetAuthOptionsAsync();
    }
}
