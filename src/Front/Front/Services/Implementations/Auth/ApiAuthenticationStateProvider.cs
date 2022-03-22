using System.Net;
using System.Security.Claims;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Front.Clients.Interfaces;
using Front.ClientsDomain.Responses;
using Front.Domain.Responses;
using Microsoft.AspNetCore.Components.Authorization;

namespace Front.Servives.Implementations;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAccountClient _accountClient; 

    public ApiAuthenticationStateProvider(IAccountClient accountClient)
    {
        _accountClient = accountClient;
    }
    
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authInfo = await _accountClient.GetAuthInfoResponseAsync();
        if (!authInfo.IsSucess)
        {
            GlobalStorage.IsAuthenticated = false;
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        if (authInfo.StatusCode == HttpStatusCode.Unauthorized)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var claims = GetClaims(authInfo.SucessResponse.Response);

        if (claims.Count > 0)
            GlobalStorage.IsAuthenticated = true;

        var authenticationUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "apiAuth"));
        return new AuthenticationState(authenticationUser);
    }

    public void MarkUserAsAuthenticated(string userName)
    {
        var authenticationUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, userName)
        }, "apiAuth"));

        var authState = Task.FromResult(new AuthenticationState(authenticationUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        var anonimusUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonimusUser));
        NotifyAuthenticationStateChanged(authState);
    }

    private List<Claim> GetClaims(AuthInfoResponse authInfo)
    {
        var result = new List<Claim>();
        result.Add(new Claim(ClaimTypes.NameIdentifier, authInfo.UserId.ToString()));
        result.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, authInfo.UserName));
        result.Add(new Claim(ClaimTypes.Name, authInfo.UserName));
        if (!string.IsNullOrEmpty(authInfo.SessionId))
            result.Add(new Claim("sessionId", authInfo.SessionId));

        foreach (var role in authInfo.Roles)
            result.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));

        return result;
    }
}