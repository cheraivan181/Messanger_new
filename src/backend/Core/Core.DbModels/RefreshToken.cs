using System.Runtime.Serialization;

namespace Core.DbModels
{
    public class RefreshToken
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public long UserID { get; set; }
        public User User { get; set; }

        [DataMember]
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
