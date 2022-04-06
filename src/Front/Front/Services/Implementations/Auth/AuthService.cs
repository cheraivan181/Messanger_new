using Blazored.LocalStorage;
using Front.Clients.Interfaces;
using Front.Domain.Auth;
using Front.Domain.FormModels;
using Front.Domain.Session;
using Front.Json;
using Front.Services.Interfaces.Auth;
using Front.Servives.Implementations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Front.Services.Implementations.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAccountClient _accountClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;

        
        public AuthService(ILocalStorageService localStorageService,
            IAccountClient accountClient,
            AuthenticationStateProvider authenticationStateProvider,
            NavigationManager navigationManager)
        {
            _accountClient = accountClient;
            _localStorageService = localStorageService;
            _authenticationStateProvider =  authenticationStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<SignInResult> SignInAsync(AuthModel authModel)
        {
            var result = new SignInResult();
            var session = (await _localStorageService.GetItemAsStringAsync(Constants.SessionName))
                ?.FromJson<SessionModel>();
            
            var response = await _accountClient.SignInAsync(authModel.UserName, authModel.Password, session?.SessionId);

            if (!response.IsSucess)
            {
                if (response.IsCanHandleError)
                    result.ErrorMessage = response.ErrorResponse.Error;
                return result;
            }

            result.IsSucess = true;

            await _localStorageService
                .SetItemAsStringAsync(Constants.TokenUpdateTime, DateTime.Now.ToString());
            await _localStorageService
                .SetItemAsStringAsync(Constants.RefreshTokenName, response.SucessResponse.Response.RefreshToken);

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(authModel.UserName);
            return result;
        }

        public async Task<SignInResult> SignUpAsync(RegisterModel registerModel)
        {
            var result = new SignInResult();
            var response = await _accountClient.SignUpAsync(registerModel.UserName, registerModel.Password,
                registerModel.Phone, registerModel.Email);

            if (!response.IsSucess)
            {
                result.ErrorMessage = response.ErrorResponse.Error;
                return result;
            }

            result.IsSucess = true;

            await _localStorageService
                .SetItemAsStringAsync(Constants.TokenUpdateTime, DateTime.Now.ToString());
            await _localStorageService
                .SetItemAsStringAsync(Constants.AcessTokenName, response.SucessResponse.Response.AcessToken);
            await _localStorageService
                .SetItemAsStringAsync(Constants.RefreshTokenName, response.SucessResponse.Response.RefreshToken);

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(registerModel.UserName);

            return result;
        }

        public async Task UpdateRefreshTokenAsync()
        {
            var refreshToken = await _localStorageService.GetItemAsStringAsync(Constants.RefreshTokenName);
            var session = (await _localStorageService.GetItemAsStringAsync(Constants.SessionName)).FromJson<SessionModel>();

            if (string.IsNullOrEmpty(refreshToken))
            {
                _navigationManager.NavigateTo("signin");
                return;
            }

            var response = await _accountClient.UpdateRefreshTokenAsync(refreshToken, session?.SessionId);
            if (!response.IsSucess)
            {
                _navigationManager.NavigateTo("signin");
                return;
            }

            await _localStorageService
                .SetItemAsStringAsync(Constants.TokenUpdateTime, DateTime.Now.ToString());
            await _localStorageService
                .SetItemAsStringAsync(Constants.AcessTokenName, response.SucessResponse.Response.AcessToken);
            await _localStorageService
                .SetItemAsStringAsync(Constants.RefreshTokenName, response.SucessResponse.Response.RefreshToken);
        }

        public async Task Logout()
        {
            var itemsToRemove = new string[]
            {
                Constants.TokenUpdateTime,
                Constants.AcessTokenName,
                Constants.RefreshTokenName,
                Constants.SessionName
            };

            await _localStorageService.RemoveItemsAsync(itemsToRemove);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _navigationManager.NavigateTo("/signin");
        }
    }
}
