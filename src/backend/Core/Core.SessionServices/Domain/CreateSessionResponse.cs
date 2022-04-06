namespace Core.SessionServices.Domain;

public class CreateSessionResponse
{
    public long SessionId { get; set; }
    public bool IsSucess { get; set; }
    public string ServerPublicKey { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsNeedUpdateToken { get; set; } 

    public void SetError(string message)
    {
        ErrorMessage = message;
        IsSucess = false;
    }

    public void SetInternalServerError()
    {
        ErrorMessage = "Internal server error";
        IsSucess = false;
    }

    public void SetSucessResult(string serverPublicKey,
        long sessionId,
        bool isNeedUpdateToken)
    {
        IsSucess = true;
        SessionId = sessionId;
        IsNeedUpdateToken = isNeedUpdateToken;
        ServerPublicKey = serverPublicKey;
    }
}