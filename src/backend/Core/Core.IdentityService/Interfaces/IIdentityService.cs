using Core.IdentityService.Domain;

namespace Core.IdentityService.Interfaces
{
    public interface IIdentityService
    {
        Task<SignUpResult> SignUpAsync(string userName,
            string phone,
            string email,
            string password);
        Task<SignInResult> SignInAsync(string userName, string password, long? sessionId);
        Task<SignInResult> UpdateJwtAsync(string refreshToken, long? sessionId);
    }
}
