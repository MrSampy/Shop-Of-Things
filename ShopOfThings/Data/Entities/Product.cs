using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Product: BaseEntity
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public decimal Price { get; set; }
        public int StorageTypeId { get; set; }
        public StorageType? StorageType { get; set; }
        public decimal Amount { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<ReceiptDetail>? ReceiptDetails { get; set; }


    }
}
