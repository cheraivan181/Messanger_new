namespace Core.DialogServices.Domain;

public class ProcessDialogRequestResult
{
    public bool IsSucess { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsAccepted { get; set; }

    public void Accept()
    {
        IsSucess = true;
        IsAccepted = true;
    }

    public void Reject()
    {
        IsSucess = true;
        IsAccepted = false;
    }

    public void SetError(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public void SetServerError()
    {
        ErrorMessage = "Server error. Try later";
    }
}