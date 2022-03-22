namespace Core.ApiResponses.Account;

public class TokenLifeTimesOptionsResponse
{
    public int CountMinutesInAcessToken { get; set; }
    public int CountMinutesInRefreshToken { get; set; }
}