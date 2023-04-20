using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class OrderDetails: BaseEntity
    {
        public Product Product { set; get; }

        public Order Order { set; get; }

        public decimal Quantity { set; get; }

        public decimal UnuitPrice { set; get; }

    }
}
