using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DbModels
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public long UserID { get; set; }
        public User User { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
