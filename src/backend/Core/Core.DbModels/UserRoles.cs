namespace Core.DbModels
{
    public class UserRoles
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
