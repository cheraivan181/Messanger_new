namespace Core.DbModels;

public class Dialog
{
    public long Id { get; set; }
    public long User1Id { get; set; }
    public long User2Id { get; set; }
    public DateTime Created { get; set; }

    public Dialog()
    {
        Created = DateTime.Now;
    }
}