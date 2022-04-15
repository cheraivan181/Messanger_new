using Core.ApiResponses.Account;
using Core.IdentityService.Domain;

namespace Core.Mapping.Interfaces
{
    public interface IIdentityServiceMapper
    {
        SignUpResponse Map(SignUpResult result);
        SignInResponse Map(SignInResult model);
    }
}
