using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Order: BaseEntity
    {
        public DateTime OperationDate { get; set; }

        public OrderStatus OrderStatus { set; get; }

        public User MasterOfOrder { get; set; }

    }
}
