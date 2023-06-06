using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IStatisticService
    {
        public Task<Dictionary<ProductCategoryModel, int>> GetNumberOfProductsInCategories();
        public Task<IEnumerable<ProductModel>> GetMostPopularProducts();
        public Task<decimal> GetIncomeOfCategoryInPeriod(Guid productCategoryId, DateTime startDate, DateTime endDate);
        public Task<decimal> GetAverageOfProductCategory(Guid productCategoryId);
        public Task<List<UserAgeCategoryModel>> GetNumberOfUsersOfEveryAgeCategory();
        public Task<ActtiveUsersModel> GetMostActtiveUsers();
    }
}
