namespace Core.CryptProtocol.Domain.Base.Responses;

public enum ResponseAction
{
    DirectMessage = 0,
    ReadMessage = 1,
    CreateDialog = 2,
    UpdateMessage = 3,
    GetDialogMessages = 4,
    Error = 5
}