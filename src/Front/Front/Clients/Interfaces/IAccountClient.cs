using Front.ClientsDomain.Responses;
using Front.Domain.Responses;
using Front.Domain.Responses.Base;

namespace Front.Clients.Interfaces;

public interface IAccountClient
{
    Task<RestClientResponse<SignInResponse>> UpdateRefreshTokenAsync(string refreshToken, Guid? sessionId);
    Task<RestClientResponse<AuthOptions>> GetAuthOptionsAsync();
    Task<RestClientResponse<AuthInfoResponse>> GetAuthInfoResponseAsync();
    Task<RestClientResponse<SignInResponse>> SignInAsync(string userName, string password, Guid? sessionId);
    Task<RestClientResponse<SignInResponse>> SignUpAsync(string userName, string password,
            string phone, string email);
}