using Core.ApiResponses.Account;
using Core.IdentityService.Domain;
using Core.Mapping.Interfaces;

namespace Core.Mapping.Impl
{
    public class IdentityServiceMapper : IIdentityServiceMapper
    {
        public SignUpResponse MapSignUpResponse(SignUpResult model)
        {
            var result = new SignUpResponse();
            result.AcessToken = model.AcessToken;
            result.RefreshToken = model.RefreshToken;

            return result;
        }

        public SignInResponse MapSignInResponse(SignInResult model)
        {
            var result = new SignInResponse();
            result.AcessToken = model.AcessToken;
            result.RefreshToken = model.RefreshToken;

            return result;
        }
    }
}
