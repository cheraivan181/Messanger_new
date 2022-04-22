using Core.CryptProtocol.Domain.Base.Responses;

namespace Core.CryptProtocol.Domain;

public class ParsedResult<T> where T:class
{
    public T RequestModel { get; set; }
    public ResponseCode ResponseCode { get; set; }
    
    public bool IsSucess => 
        ResponseCode == ResponseCode.Sucess;
}