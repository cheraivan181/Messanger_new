namespace Front.Domain.Session
{
    public class SessionModel
    {
        public Guid SessionId { get; set; }
        public string ServerPublicKey { get; set; }
        public string ClientPublicKey { get; set; }
        public string ClientPrivateKey { get; set; }
        public string SignKey { get; set; }
        public string Aes { get; set; }
    }
}
