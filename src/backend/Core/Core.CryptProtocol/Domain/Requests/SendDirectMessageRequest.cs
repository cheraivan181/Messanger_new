namespace Core.CryptProtocol.Domain;

public class SendDirectMessageRequest
{
    public Guid ToId { get; set; }
    public Guid FromId { get; set; }
    public string Text { get; set; }
}