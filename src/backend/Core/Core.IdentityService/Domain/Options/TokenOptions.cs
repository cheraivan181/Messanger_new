namespace Core.IdentityService.Domain.Options
{
	public class TokenLifeTimeOptions
	{
		public int RefreshTokenLifeTime { get; set; }
		public int AccessTokenLifeTime { get; set; }
	}
}
