using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.DbModels
{
    public class RefreshToken
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; }
        public User User { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }

        public RefreshToken()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
