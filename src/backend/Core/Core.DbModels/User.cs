using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModels
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(50)]
        public string Phone { get; set; }
        [MaxLength(300)]
        public string UserName { get; set; }
        [MaxLength(300)]
        public string Email { get; set; }
        [MaxLength(300)]
        public string Password { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public List<Connection> Connections { get; set; }
        public DateTime CreatedAt { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
        }
    } 
}
