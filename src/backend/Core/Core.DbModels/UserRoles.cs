using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DbModels
{
    public class UserRoles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }

        public UserRoles()
        {
            Id = Guid.NewGuid();
        }
    }
}
