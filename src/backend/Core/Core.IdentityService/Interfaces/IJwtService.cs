namespace Core.IdentityService.Interfaces
{
    public interface IJwtService
    {
        string GenereteJwtToken(string userName,
            long userid,
            List<string> roles,
            long? sessionId = null);
        string GenarateRefreshToken();
    }
}
