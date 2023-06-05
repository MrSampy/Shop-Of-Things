using Business.Models;
using Business.Services;
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
    public class RecommendationIntegrationTest
    {
        private HttpClient client;
        private const string RequestUri = "api/user/";
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
        public async Task RecommendationController_GetProductsByRecent()
        {
            //Arrange
            const int expected = 2;
            await apiBuilder.LogIn("123", "123");

            //Act
            var users = await apiBuilder.GetRequest<IEnumerable<UserModel>>(RequestUri);
            var userId = users.Last().Id;
            var products = await apiBuilder.GetRequest<IEnumerable<ProductModel>>($"{RequestUri}recommendation/{userId}/products");
            var actual = products.Count();

            //Assert
            Assert.AreEqual(expected,actual);
        }

        [TestMethod]
        public async Task RecommendationController_GetReceiptsByProducts()
        {
            //Arrange
            const int expected = 4;
            await apiBuilder.LogIn("123", "123");

            //Act
            var users = await apiBuilder.GetRequest<IEnumerable<UserModel>>(RequestUri);
            var userId = users.Last().Id;
            var products = await apiBuilder.GetRequest<IEnumerable<RecommendationReceiptsModel>>($"{RequestUri}recommendation/{userId}/receipts");
            var actual = products.Count();

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
