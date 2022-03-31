using Core.DbModels.Base.Interface;

namespace Core.Repositories.Impl;

public class SessionRepository
{
    private readonly IConnectionFactory _connectionFactory;
    
    public SessionRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
}