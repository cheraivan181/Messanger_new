﻿namespace Core.SessionServices.Services.Interfaces;

public interface IConnectionCollectorService
{
    Task AddConnectionAsync(Guid userId, Guid sessionId, string connectionId);
    Task RemoveConnectionAsync(Guid userId, string connectionId);
}