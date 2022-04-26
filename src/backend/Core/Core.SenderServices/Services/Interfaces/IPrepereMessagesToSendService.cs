using Core.BinarySerializer;
using Core.CryptProtocol.Domain.Base.Responses;
using Core.SenderServices.Domain;

namespace Core.MessageServices.Services.Interfaces;

public interface IPrepereMessagesToSendService
{
    Task<MessageToSendInNetwork> BuildMessageToSendResponseAsync<T>(T responseModel,
        Guid userId, ResponseAction responseAction, ResponseCode code = ResponseCode.Sucess)
        where T : class, ISerializableMessage;

    Task<MessageToSendInNetwork> BuildMessageForSingleConnection<T>(T responseModel, Guid userId,
        string connectionId, ResponseAction responseAction, ResponseCode responseCode)
        where T : class, ISerializableMessage;
}