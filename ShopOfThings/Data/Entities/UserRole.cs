using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class UserRole:BaseEntity
    {
        public string UserRoleName { set; get; }

        public ICollection<User> Users { get; set; }
    }
}
