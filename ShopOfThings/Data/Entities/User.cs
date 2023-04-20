using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class User: BaseEntity
    {
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string SecondName { set; get; }
        public DateTime BirthDate { set; get; }

    }
}
