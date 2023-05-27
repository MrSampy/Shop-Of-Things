using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ProductCategory:BaseEntity
    {
        public string ProductCategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
