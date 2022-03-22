namespace Core.ApiResponses.Account
{
    public class AuthInfoResponse
    {
        public string UserName { get; set; }
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
        public string TokenId { get; set; }
        public string SessionId { get; set; }
    }
}
