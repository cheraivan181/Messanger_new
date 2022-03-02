namespace Core.DbModels
{
    public class User
    {
        public long Id { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public DateTime CreatedAt { get; set; }
    } 
}
