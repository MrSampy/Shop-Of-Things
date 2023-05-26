using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ProductCategoryModel
    {
        public Guid Id { get; set; }

        public string ProductCategoryName { set; get; }

        public ICollection<Guid> ProductsIds { set; get; }
    }
}
