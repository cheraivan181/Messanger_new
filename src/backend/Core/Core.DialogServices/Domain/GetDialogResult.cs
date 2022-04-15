namespace Core.DialogServices.Domain;

public class GetDialogResult
{
    public bool IsSucess { get; set; }
    public string ErrorMessage { get; set; }
    public List<Dialog> Dialogs { get; set; }
    public int Count { get; set; }

    public void SetSucessResult(List<Dialog> dialogs)
    {
        IsSucess = true;
        Dialogs = dialogs;
        Count = Dialogs.Count;
    }

    public void SetEmptySucessResult()
    {
        IsSucess = true;
    }

    public void SetError(string message)
    {
        ErrorMessage = message;
    }
    
    public void SetServerError()
    {
        ErrorMessage = "Internal server error. Try later";
    }
    
    public record Dialog(Guid userId,Guid dialogId, string userName, string cypherKey, string iv, bool isConfirmDialog);
}