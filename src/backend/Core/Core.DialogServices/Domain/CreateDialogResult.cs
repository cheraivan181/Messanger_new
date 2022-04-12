namespace Core.DialogServices.Domain;

public class CreateDialogResult
{
    public bool IsSucess { get; set; }
    
    public string ErrorMessage { get; set; }
    
    public string DialogCryptedKey { get; set; }
    public Guid CreatedDialogId { get; set; }

    public void SetResult(Guid dialogId)
    {
        CreatedDialogId = dialogId;
    }
}