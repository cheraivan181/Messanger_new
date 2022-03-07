using Core.ApiResponses.Account;
using Core.IdentityService.Domain;

namespace Core.Mapping.Interfaces
{
    public interface IIdentityServiceMapper
    {
        SignUpResponse MapSignUpResponse(SignUpResult result);
        SignInResponse MapSignInResponse(SignInResult model);
    }
}
