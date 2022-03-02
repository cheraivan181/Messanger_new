using Core.DbModels;
using Core.DbModels.Base.Interface;
using Dapper;

namespace Core.Repositories
{
    public class UserRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public UserRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User> GetUserByPhoneAndPassword(string userName, string hashPassword)
        {
            using var conn = await _connectionFactory.GetDbConnectionAsync();
            string sql = "SELECT * FROM Users WHERE userName = @userName and Password = @hashPasword";

            var user = await conn.QueryFirstOrDefaultAsync(sql, new
            {
                userName = userName,
                hashPassword = hashPassword
            });

            return user;
        }
    }
}
