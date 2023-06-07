using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ActiveUsersModel
    {
        public Guid UserIdWithMostReceipts { set; get; }
        public Guid UserIdWithMostOrders { set; get; }
        public Guid UserIdWithMostProducts { set; get; }

    }
}
