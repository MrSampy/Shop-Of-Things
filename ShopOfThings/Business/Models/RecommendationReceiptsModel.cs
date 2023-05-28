using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class RecommendationReceiptsModel
    {
        public ReceiptModel Receipt { get; set; }
        public IEnumerable<ProductModel> MissingProducts { get; set; }

    }
}
