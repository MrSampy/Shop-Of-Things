using Business.Models;
using Data.Data;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopOfThings.Tests.UnitTestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;


namespace ShopOfThings.Tests.Integration
{
    [TestClass]
    public class StatisticIntegrationTest
    {
        private HttpClient client;
        private const string RequestUri = "api/statistic/";
        private CustomWebApplicationFactory factory;
        private ApiBuilder apiBuilder;

        [TestInitialize]
        public void Init()
        {
            factory = new CustomWebApplicationFactory();
            client = factory.CreateClient();
            apiBuilder = new ApiBuilder(client);
        }

        [TestMethod]
        public async Task StatisticController_GetNumberOfUsersOfEveryAgeCategory()
        {
            //Arrange
            const int expected = 3;
            await apiBuilder.LogIn("123", "123");

            //Act
            var users = await apiBuilder.GetRequest<List<UserAgeCategoryModel>>($"{RequestUri}agecategories");
            var actual = users.ElementAt(1).Users.Count();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task StatisticController_GetNumberOfProductsInCategories()
        {
            //Arrange
            await apiBuilder.LogIn("123", "123");

            //Act
            var productCategories = await apiBuilder.GetRequest<IEnumerable<ProductCategoryModel>>("api/Product/category");
            var actual = await apiBuilder.GetRequest<List<ProductCategoryNumberModel>>($"{RequestUri}productincategories");

            //Assert
            foreach (var productCategory in productCategories)
            {
                Assert.AreEqual(productCategory.ProductsIds.Count(), actual.First(x => x.CategoryName.Equals(productCategory.ProductCategoryName)).Number);
            }
        }

        [TestMethod]
        public async Task StatisticController_GetMostPopularProducts()
        {
            //Arrange
            await apiBuilder.LogIn("123", "123");

            //Act
            var products = await apiBuilder.GetRequest<IEnumerable<ProductModel>>("api/Product");
            var actual = await apiBuilder.GetRequest<IEnumerable<ProductModel>>($"{RequestUri}mostpopularproducts");
            //Assert
            Assert.AreEqual(products.ElementAt(0).Id, actual.First().Id);
        }

        [TestMethod]
        public async Task StatisticController_GetMostActtiveUsers()
        {
            //Arrange
            await apiBuilder.LogIn("123", "123");
            //Act
            var users = await apiBuilder.GetRequest<IEnumerable<UserModel>>("api/user");
            var actual = await apiBuilder.GetRequest<ActiveUsersModel>($"{RequestUri}mostactiveuser");
            //Assert
            Assert.AreEqual(users.Last().Id, actual.UserIdWithMostOrders);
            Assert.AreEqual(users.Last().Id, actual.UserIdWithMostProducts);
            Assert.AreEqual(users.ElementAt(1).Id, actual.UserIdWithMostReceipts);
        }

        [TestMethod]
        public async Task StatisticController_GetIncomeOfCategoryInPeriod()
        {
            //Arrange
            await apiBuilder.LogIn("123", "123");
            const string expected = "109.0";
            //Act
            var productCategories = await apiBuilder.GetRequest<IEnumerable<ProductCategoryModel>>("api/Product/category");
            var productsInPeriodModel = new ProductsInPeriodModel 
            {
                ProductCategoryId = productCategories.First().Id,
                StartDate = DateTime.Today.AddDays(-10),
                EndDate = DateTime.Today.AddDays(-2)
            };
            var actual = await apiBuilder.PostRequest<string>($"{RequestUri}incomeofcategoryperiod",productsInPeriodModel);            
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task StatisticController_GetIncomeOfCategoryInPeriod_Zero()
        {
            //Arrange
            await apiBuilder.LogIn("123", "123");
            const string expected = "0";
            //Act
            var productCategories = await apiBuilder.GetRequest<IEnumerable<ProductCategoryModel>>("api/Product/category");
            var productsInPeriodModel = new ProductsInPeriodModel
            {
                ProductCategoryId = productCategories.First().Id,
                StartDate = DateTime.Today.AddDays(-3),
                EndDate = DateTime.Today.AddDays(-2)
            };
            var actual = await apiBuilder.PostRequest<string>($"{RequestUri}incomeofcategoryperiod", productsInPeriodModel);
            //Assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public async Task StatisticController_GetAverageOfProductCategory()
        {
            //Arrange
            await apiBuilder.LogIn("123", "123");
            const string expected = "15.9";
            //Act
            var productCategories = await apiBuilder.GetRequest<IEnumerable<ProductCategoryModel>>("api/Product/category");
            var id = productCategories.First().Id.ToString();
            var actual = await apiBuilder.PostRequest<string>($"{RequestUri}avproductcategory/"+id, id);
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
