using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRecommendationService
    {
        public Task<IEnumerable<ProductModel>> GetProductsByRecent(Guid userId);

        public Task<IEnumerable<RecommendationReceiptsModel>> GetReceiptsByProducts(Guid userId);

    }
}
