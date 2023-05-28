using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class OrderStatus: BaseEntity
    {
        public string OrderStatusName { set; get; }
        public ICollection<Order> Orders { get; set; }

    }
}
