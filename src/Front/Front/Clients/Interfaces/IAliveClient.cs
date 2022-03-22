using Front.Domain.Responses;
using Front.Domain.Responses.Base;

namespace Front.Clients.Interfaces;

public interface IAliveClient
{
    Task<RestClientResponse<IsAliveResponse>> IsAliveAsync();
}