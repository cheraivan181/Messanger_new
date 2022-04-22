namespace Core.DialogServices.Domain;

public class CreateDialogRequestResult
{
    public bool IsSucess { get; set; }
    public Guid DialogRequestId { get; set; }
    public Guid DialogId { get; set; }
    public string Key { get; set; }
    public string ErrorMessage { get; set; }

    public void SetSucessResult(Guid dialogRequestId, Guid dialogId, string key)
    {
        DialogRequestId = dialogRequestId;
        DialogId = dialogId;
        Key = key;
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
}