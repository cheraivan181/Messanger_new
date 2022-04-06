using Front.Domain.Auth;
using Front.Domain.FormModels;

namespace Front.Services.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<SignInResult> SignInAsync(AuthModel authModel);
        Task<SignInResult> SignUpAsync(RegisterModel registerModel);
        Task UpdateRefreshTokenAsync();
        Task Logout();
    }
}
