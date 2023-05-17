using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class UserStatus:BaseEntity
    {
        public string UserStatusName { set; get; }

        public ICollection<User>? Users { get; set; }
    }
}
