namespace Core.IdentityService.Interfaces
{
    public interface IJwtService
    {
        string GenereteJwtToken(string userName,
            Guid userid,
            List<string> roles,
            Guid? sessionId = null);
        string GenarateRefreshToken();
    }
}
