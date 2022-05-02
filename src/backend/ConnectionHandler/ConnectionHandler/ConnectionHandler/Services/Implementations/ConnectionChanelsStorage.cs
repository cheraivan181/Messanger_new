using System.Threading.Channels;
using ConnectionHandler.Services.Interfaces;


namespace ConnectionHandler.Services.Implementations;

public class ConnectionChanelsStorage : IConnectionChanelsStorage
{
    private Dictionary<string, Channel<string>> _connectionChannels = new Dictionary<string, Channel<string>>();

    private Dictionary<string, CancellationTokenSource> _cancellationTokens =
        new Dictionary<string, CancellationTokenSource>();


    public Channel<string> GetConnectionChannel(string connectionId)
    {
        if (_connectionChannels.TryGetValue(connectionId, out var channel))
            return channel;

        throw new Exception($"Cannot get channel #({connectionId})");
    }
    
    public void AddChannel(string connectionId)
    {
        _connectionChannels.TryAdd(connectionId, Channel.CreateUnbounded<string>());
        _cancellationTokens.TryAdd(connectionId, new CancellationTokenSource());
    }
    
    public void RemoveChannel(string connectionId)
    {
        _connectionChannels.Remove(connectionId);
        if (_cancellationTokens.TryGetValue(connectionId, out var tokenSource))
        {
            tokenSource.Cancel();
            _cancellationTokens.Remove(connectionId);
        }
        else
            throw new Exception($"Cannot get cancel token #({connectionId})");
    }
    
    
    public CancellationTokenSource GetToken(string connectionId)
    {
        if (_cancellationTokens.TryGetValue(connectionId, out var token))
            return token;

        throw new Exception($"Cannot get cancel token #({connectionId})");
    }
}