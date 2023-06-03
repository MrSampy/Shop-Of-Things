using AutoMapper;
using Business.Models;
using Business.Validation;
using Data.Interfaces;
using Data.Entities;
using Business.Interfaces;

namespace Business.Services
{
    public class RecommendationService: IRecommendationService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;
        public RecommendationService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }

        public async Task<IEnumerable<ProductModel>> GetProductsByRecent(Guid userId) 
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(userId) ?? throw new ShopOfThingsException("User not found!");
            List<Product> result = new List<Product>();
            for (int orderId = 0; orderId < user.Orders.Count; ++orderId) 
            {
                foreach (var orderDetail in user.Orders.ToList()[orderId].OrderDetails) 
                {
                    result.AddRange(UnitOfWork.ProductRepository.GetAllAsync().Result.Where(x=>!result.Any(y=>y.Id.Equals(x.Id))
                    && x.ProductCategoryId.Equals(orderDetail.Product.ProductCategoryId) && !x.UserId.Equals(user.Id) && x.Amount!=0));
                }                
            }

            return Mapper.Map<IEnumerable<ProductModel>>(result); 
        }

        public async Task<IEnumerable<RecommendationReceiptsModel>> GetReceiptsByProducts(Guid userId)
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(userId) ?? throw new ShopOfThingsException("User not found!");

            var receipts = UnitOfWork.ReceiptRepository.GetAllAsync().Result.Where(receipt => !receipt.UserId.Equals(userId) &&
             user.Products.ToList().Intersect(receipt.ReceiptDetails.Select(x => x.Product).ToList()).Count() > receipt.ReceiptDetails.Count/3).ToList();

            var result = new List<RecommendationReceiptsModel>();

            foreach (var receipt in receipts) 
            {
                result.Add(new RecommendationReceiptsModel 
                {
                    Receipt = Mapper.Map<ReceiptModel>(receipt),
                    MissingProducts = Mapper.Map<IEnumerable<ProductModel>>(receipt.ReceiptDetails.Select(x => x.Product).Where(x => !x.UserId.Equals(userId)))
                });
            }

            return result;
        }
    }
}
