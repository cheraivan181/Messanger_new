namespace Core.CryptProtocol.Domain.Base.Responses;

public enum ResponseCode
{
    Sucess = 100,
    InvalidModel = 400,
    CannotDeserializeRequestMessage = 401,
    InvalidSign = 500,
    InvalidCryptText = 501,
    GlobalError = 1000
}