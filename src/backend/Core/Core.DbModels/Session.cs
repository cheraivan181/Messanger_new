namespace Core.DbModels;

public class Session
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string ClientPublicKey { get; set; }
    public string ServerPublicKey { get; set; }
    public string ServerPrivateKey { get; set; }
    public User User { get; set; }
}