using System.ComponentModel.DataAnnotations;

namespace Core.DbModels
{
    public class User
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(300)]
        public string UserName { get; set; }
        [MaxLength(300)]
        public string Email { get; set; }
        [MaxLength(300)]
        public string Password { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public DateTime CreatedAt { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
        }
    } 
}
