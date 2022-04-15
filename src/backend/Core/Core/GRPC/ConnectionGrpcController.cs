using Grpc.Core;

namespace Core.GRPC;

public class ConnectionGrpcController
{
    public async Task ConnectUserAsync(ServerCallContext serverCallContext){}
    public async Task UserDisconnectAsync(ServerCallContext serverCallContext){}
}