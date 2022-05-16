namespace Core.DbModels;

public class UserCypherKey
{
    public Guid Id { get; set; }
    
    public string CryptedKey { get; set; }
    
    public Guid SessionId { get; set; }

    public DateTime CreatedAt { get; set; }
}