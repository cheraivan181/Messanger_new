using Core.CryptProtocol.Domain.Base.Responses;

namespace Core.CryptProtocol.Domain;

public class ParsedMessageModel
{
    public string DecryptedText { get; set; }
    public bool IsSucess { get; set; }
}