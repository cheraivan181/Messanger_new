using Core.DbModels;
using Core.MessageServices.Domain;

namespace Core.MessageServices.Services.Interfaces;

public interface IMessageMapper
{
    MessageModel Map(Message message);
}