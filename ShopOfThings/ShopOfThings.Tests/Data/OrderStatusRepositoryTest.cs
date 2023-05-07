using Data.Data;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using ShopOfThings.Tests.UnitTestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOfThings.Tests.Data
{
    [TestClass]
    public class OrderStatusTest
    {
        public async Task<OrderStatusRepository> CreateRepositoryAsync()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new OrderStatusRepository(context);

        }

        [DataTestMethod]
        [DataRow("Payment_received", 2)]
        [DataRow("Sent", 3)]
        public async Task StorageTypeRepository_GetByIdAsync(string expected, int id)
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            //Act
            var actual = await repository.GetByIdAsync(id);
            //Assert
            Assert.AreEqual(expected, actual.OrderStatusName);

        }

        [TestMethod]
        public async Task StorageTypeRepository_GetAllAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            int expectedLength = 7;
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength, actual.Count());
        }


        [TestMethod]
        public async Task StorageTypeRepository_AddAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            int expectedLength = 8;
            var newStartus = new OrderStatus { OrderStatusName = "Status" };
            //Act
            await repository.AddAsync(newStartus);
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength, actual.Count());
        }

        [TestMethod]
        public async Task StorageTypeRepository_UpdateAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            string expectedStatusName = "Status";

            var newStatus = new OrderStatus { Id = 1, OrderStatusName = expectedStatusName };

            repository.Update(newStatus);
            //Act
            var actual = await repository.GetByIdAsync(newStatus.Id);
            //Assert
            Assert.AreEqual(expectedStatusName, actual.OrderStatusName);
        }

        [TestMethod]
        public async Task StorageTypeRepository_DeleteAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedLen = 6;

            var entitToDelete = await repository.GetByIdAsync(1);

            repository.Delete(entitToDelete);
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }

        [TestMethod]
        public async Task StorageTypeRepository_DeleteByIdAsync()
        {
            //Arrange
            var userStatusRepository = await CreateRepositoryAsync();

            var expectedLen = 6;

            await userStatusRepository.DeleteByIdAsync(1);
            //Act
            var actual = await userStatusRepository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }
    }
}
