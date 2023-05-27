using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class OrderDetailPriceModel
    {
        public Guid OrderDetailId { set; get; }
        public decimal UnitPrice { set; get; }
    }
}
