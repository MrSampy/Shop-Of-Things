using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Business.Validation;
using Data.Entities;
using System.Text.RegularExpressions;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;
        private readonly List<int> Ages = new List<int> {0,18,30,50,70,150 };
        public StatisticService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }

        public async Task<decimal> GetAverageOfProductCategory(Guid productCategoryId)
        {
            _ = await UnitOfWork.ProductCategoryRepository.GetByIdAsync(productCategoryId) ?? throw new ShopOfThingsException("Product category not found!");

            return UnitOfWork.ProductRepository.GetAllAsync().Result.Where(x => x.ProductCategoryId.Equals(productCategoryId)).Select(x => x.Price).Sum();
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(Guid productCategoryId, DateTime startDate, DateTime endDate)
        {
            _ = await UnitOfWork.ProductCategoryRepository.GetByIdAsync(productCategoryId) ?? throw new ShopOfThingsException("Product category not found!");
            return UnitOfWork.OrderRepository.GetAllAsync().Result.Where(x => x.OperationDate >= startDate && x.OperationDate <= endDate)
                .SelectMany(x => x.OrderDetails).Where(x => x.Product.ProductCategoryId.Equals(productCategoryId))
                .Select(x => x.Quantity * x.Product.Price).Sum();
        }

        public Task<ActiveUsersModel> GetMostActtiveUsers()
        {
            return Task.FromResult(new ActiveUsersModel
            {
                UserIdWithMostOrders = UnitOfWork.OrderRepository.GetAllAsync().Result.GroupBy(x=>x.UserId).Select(g=>new { UserId = g.Key, Count = g.Count() }).OrderByDescending(x=>x.Count).First().UserId,
                UserIdWithMostProducts = UnitOfWork.ProductRepository.GetAllAsync().Result.GroupBy(x => x.UserId).Select(g => new { UserId = g.Key, Count = g.Count() }).OrderByDescending(x => x.Count).First().UserId,
                UserIdWithMostReceipts = UnitOfWork.ReceiptRepository.GetAllAsync().Result.GroupBy(x => x.UserId).Select(g => new { UserId = g.Key, Count = g.Count() }).OrderByDescending(x => x.Count).First().UserId
            });
        }

        public Task<IEnumerable<ProductModel>> GetMostPopularProducts()
        {
            return Task.FromResult(Mapper.Map<IEnumerable<ProductModel>>(UnitOfWork.OrderRepository.GetAllAsync().Result.SelectMany(x => x.OrderDetails)
                .GroupBy(x => x.Product).Select(g => new { Product = g.Key, Count = g.Count() }).OrderByDescending(x => x.Count).Select(x => x.Product)));
        }

        public Task<List<ProductCategoryNumberModel>> GetNumberOfProductsInCategories()
        {
            return Task.FromResult(UnitOfWork.ProductRepository.GetAllAsync().Result.GroupBy(x => x.ProductCategory).Select(g => new ProductCategoryNumberModel { CategoryName = g.Key.ProductCategoryName, Number = g.Count() }).ToList() );
        }

        public Task<List<UserAgeCategoryModel>> GetNumberOfUsersOfEveryAgeCategory()
        {
            var list = new List<UserAgeCategoryModel>();
            for(int index = 0; index < Ages.Count-1;++index)
            {
                var startYear = Ages[index];
                var endYear = Ages[index + 1];
                var ageCategory = new UserAgeCategoryModel
                {
                    StartYear = startYear,
                    EndYear = endYear,
                    Users = Mapper.Map<IEnumerable<UserModel>>(UnitOfWork.UserRepository.GetAllAsync().Result.Where(x =>
                    (DateTime.Today.Year - x.BirthDate.Year) >= startYear && (DateTime.Today.Year - x.BirthDate.Year) < endYear))
                };
                list.Add(ageCategory);
            }
            return Task.FromResult(list);
        }
    }
}