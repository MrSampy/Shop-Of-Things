using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ShopOfThings.Tests.UnitTestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopOfThings.Tests.Business
{
    [TestClass]
    public class RecommendationServiceTest
    {
        private async Task<RecommendationService> CreateService()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
               .EnableSensitiveDataLogging()
               .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new RecommendationService(UnitTestHelper.CreateUnitOfWork(context), UnitTestHelper.CreateMapper());
        }

        [TestMethod]
        public async Task RecommendationService_GetProductsByRecent()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 2;
            //Act
            var user = service.UnitOfWork.UserRepository.GetAllAsync().Result.Last();
            var products = await service.GetProductsByRecent(user.Id);
            var actual = products.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task RecommendationService_GetReceiptsByProducts()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 4;
            //Act
            var user = service.UnitOfWork.UserRepository.GetAllAsync().Result.Last();
            var receipts = await service.GetReceiptsByProducts(user.Id);
            var actual = receipts.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task RecommendationService_GetProductsByRecent_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.GetProductsByRecent(userId), "User not found!");
        }

        [TestMethod]
        public async Task RecommendationService_GetReceiptsByProducts_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.GetReceiptsByProducts(userId), "User not found!");
        }
    }
}
