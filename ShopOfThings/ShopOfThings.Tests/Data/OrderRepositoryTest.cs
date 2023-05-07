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
    public class OrderRepositoryTest
    {
        public async Task<OrderRepository> CreateRepositoryAsync()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new OrderRepository(context);

        }

        [DataTestMethod]
        [DataRow(3, 1)]
        [DataRow(2, 2)]
        public async Task OrderRepository_GetByIdAsync(int expected, int id)
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            //Act
            var actualEntity = await repository.GetByIdAsync(id);
            //Assert
            Assert.AreEqual(expected, actualEntity.UserId);

        }

        [TestMethod]
        public async Task OrderRepository_GetAllAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            int expectedLength = 2;
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength, actual.Count());
        }


        [TestMethod]
        public async Task OrderRepository_AddAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            int expectedLength = 3;
            var newEntity = new Order
            {
                OperationDate = DateTime.Today,
                OrderStatusId = 1,
                UserId = 2
            };
            //Act
            await repository.AddAsync(newEntity);
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength, actual.Count());
        }

        [TestMethod]
        public async Task OrderRepository_UpdateAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedProperty = DateTime.Today.AddDays(-5);

            var newStatus = await repository.GetByIdAsync(1);
            newStatus.OperationDate = expectedProperty;
            repository.Update(newStatus);
            //Act
            var actual = await repository.GetByIdAsync(newStatus.Id);
            //Assert
            Assert.AreEqual(expectedProperty, actual.OperationDate);
        }

        [TestMethod]
        public async Task OrderRepository_DeleteAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedLen = 1;

            var entitToDelete = await repository.GetByIdAsync(1);

            repository.Delete(entitToDelete);
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }

        [TestMethod]
        public async Task OrderRepository_DeleteByIdAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedLen = 1;

            await repository.DeleteByIdAsync(1);
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }
    }
}
