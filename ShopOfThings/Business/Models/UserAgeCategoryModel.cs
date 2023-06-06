using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class UserAgeCategoryModel
    {
        public int StartYear { set; get; }
        public int EndYear { set; get; }
        public IEnumerable<UserModel> Users { set; get; }

    }
}
