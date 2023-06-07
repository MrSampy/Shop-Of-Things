using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ShopOfThings.Tests.UnitTestHelpers;

namespace ShopOfThings.Tests.Business
{
    [TestClass]
    public class StatisticServiceTest
    {
        private async Task<StatisticService> CreateService()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
               .EnableSensitiveDataLogging()
               .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new StatisticService(UnitTestHelper.CreateUnitOfWork(context), UnitTestHelper.CreateMapper());
        }

        [TestMethod]
        public async Task StatisticService_GetAverageOfProductCategory()
        {
            //Arrange
            var service = await CreateService();
            decimal expected = 15.9M;
            //Act
            var productCategoryId = service.UnitOfWork.ProductCategoryRepository.GetAllAsync().Result.First().Id;
            var actual = await service.GetAverageOfProductCategory(productCategoryId);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task StatisticService_GetAverageOfProductCategory_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var productCategoryId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.GetAverageOfProductCategory(productCategoryId), "Product category not found!");
        }

        [TestMethod]
        public async Task StatisticService_GetIncomeOfCategoryInPeriod()
        {
            //Arrange
            var service = await CreateService();
            decimal expected = 109M;
            //Act
            var productCategoryId = service.UnitOfWork.ProductCategoryRepository.GetAllAsync().Result.First().Id;
            var actual = await service.GetIncomeOfCategoryInPeriod(productCategoryId, DateTime.Today.AddDays(-10), DateTime.Today.AddDays(-2));
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task StatisticService_GetIncomeOfCategoryInPeriod_Zero()
        {
            //Arrange
            var service = await CreateService();
            decimal expected = 0;
            //Act
            var productCategoryId = service.UnitOfWork.ProductCategoryRepository.GetAllAsync().Result.First().Id;
            var actual = await service.GetIncomeOfCategoryInPeriod(productCategoryId, DateTime.Today.AddDays(-3), DateTime.Today.AddDays(-2));
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task StatisticService_GetIncomeOfCategoryInPeriod_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var productCategoryId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.GetIncomeOfCategoryInPeriod(productCategoryId, DateTime.Today.AddDays(-10), DateTime.Today.AddDays(-2)), "Product category not found!");
        }

        [TestMethod]
        public async Task StatisticService_GetMostActtiveUsers()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var users = service.UnitOfWork.UserRepository.GetAllAsync().Result;
            var actual = await service.GetMostActtiveUsers();
            //Assert
            Assert.AreEqual(users.Last().Id, actual.UserIdWithMostOrders);
            Assert.AreEqual(users.Last().Id, actual.UserIdWithMostProducts);
            Assert.AreEqual(users.ElementAt(1).Id, actual.UserIdWithMostReceipts);
        }

        [TestMethod]
        public async Task StatisticService_GetMostPopularProducts()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var products = service.UnitOfWork.ProductRepository.GetAllAsync().Result;
            var actual = await service.GetMostPopularProducts();
            //Assert
            Assert.AreEqual(products.ElementAt(0).Id, actual.First().Id);
        }

        [TestMethod]
        public async Task StatisticService_GetNumberOfProductsInCategories()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var productCategories = service.UnitOfWork.ProductCategoryRepository.GetAllAsync().Result;
            var actual = await service.GetNumberOfProductsInCategories();
            //Assert
            foreach (var productCategory in productCategories) 
            {
                Assert.AreEqual(productCategory.Products.Count(),actual.First(x=>x.Key.Id.Equals(productCategory.Id)).Value);
            }
        }

        [TestMethod]
        public async Task StatisticService_GetNumberOfUsersOfEveryAgeCategory()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 3;
            //Act
            var actual = await service.GetNumberOfUsersOfEveryAgeCategory();
            //Assert
            Assert.AreEqual(expected, actual.ElementAt(1).Users.Count());
        }

    }
}