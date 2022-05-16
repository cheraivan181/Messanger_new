namespace Core.CryptProtocol.Domain.Base.Responses;

public enum ResponseCode
{
    Sucess = 100,
    IncorrectSign = 501,
    IncorrectCryptedText = 502,
    NotAvailableDialog = 503,
    GlobalError = 1000
}