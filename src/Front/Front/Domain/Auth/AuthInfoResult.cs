using Front.ClientsDomain.Responses;

namespace Front.Domain.Auth
{
    public class AuthInfoResult
    {
        public bool IsAuthorized { get; set; }
        public AuthInfoResponse AuthInfoResponse { get; set; }
    }
}
