using Business.Models;
namespace Business.Interfaces
{
    public interface IRecommendationService
    {
        public Task<IEnumerable<ProductModel>> GetProductsByRecent(Guid userId);

        public Task<IEnumerable<RecommendationReceiptsModel>> GetReceiptsByProducts(Guid userId);

    }
}
