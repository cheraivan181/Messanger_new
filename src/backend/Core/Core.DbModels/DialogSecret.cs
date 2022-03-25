namespace Core.DbModels;

public class Secret
{
    public long Id { get; set; }
    public long DialogId { get; set; }
    public string CypherKey { get; set; }
    public Dialog Dialog { get; set; }
}