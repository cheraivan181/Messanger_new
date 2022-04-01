﻿namespace Core.SessionServices.Domain;

public class SessionModel
{
    public long SessionId { get; set; }
    public string ServerPrivateKey { get; set; }
    public string ServerPublicKey { get; set; }
    public string ClientPublicKey { get; set; }

    public SessionModel(
        long sessionId,
        string serverPrivateKey,
        string serverPublicKey,
        string clientPublicKey)
    {
        SessionId = sessionId;
        ServerPrivateKey = serverPrivateKey;
        ServerPublicKey = serverPublicKey;
        ClientPublicKey = clientPublicKey;
    }
}