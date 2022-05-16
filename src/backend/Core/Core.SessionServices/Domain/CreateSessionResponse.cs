namespace Core.SessionServices.Domain;

public class CreateSessionResponse
{
    public Guid SessionId { get; set; }
    public bool IsSucess { get; set; }
    public string ServerPublicKey { get; set; }
    public string HmacKey { get; set; }
    public string AesKey { get; set; }
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
        Guid sessionId,
        string hmacKey,
        string aesKey,
        bool isNeedUpdateToken)
    {
        IsSucess = true;
        SessionId = sessionId;
        IsNeedUpdateToken = isNeedUpdateToken;
        HmacKey = hmacKey;
        AesKey = aesKey;
        ServerPublicKey = serverPublicKey;
    }
}