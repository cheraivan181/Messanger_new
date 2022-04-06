using Front.ClientsDomain.Responses.Session;
using Front.Domain.Responses.Base;

namespace Front.Clients.Interfaces
{
    public interface ISessionClient
    {
        Task<RestClientResponse<CreateSessionResponse>> CreateSessionAsync(string publicKey);
    }
}
