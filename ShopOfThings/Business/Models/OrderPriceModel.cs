using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class OrderPriceModel
    {
        public decimal FullPrice { get; set; }
        public IEnumerable<OrderDetailPriceModel> OrderDetailPrices {set; get;}
    }
}
