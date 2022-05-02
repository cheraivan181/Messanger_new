using System.Threading.Channels;

namespace ConnectionHandler.Services.Interfaces;

public interface IConnectionChanelsStorage
{
    Channel<string> GetConnectionChannel(string connectionId);
    void AddChannel(string connectionId);
    void RemoveChannel(string connectionId);
    CancellationTokenSource GetToken(string connectionId);
}