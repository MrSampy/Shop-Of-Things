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
        public User ProductOwner { get; set; }
        public decimal Price { get; set; }
        public StorageType StorageType { get; set; }
        public decimal Amount { get; set; }
    }
}
