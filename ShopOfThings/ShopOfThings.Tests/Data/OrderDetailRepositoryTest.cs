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
    public class OrderDetailRepositoryTest
    {
        public async Task<OrderDetailRepository> CreateRepositoryAsync()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new OrderDetailRepository(context);

        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 3)]
        public async Task OrderDetailRepository_GetByIdAsync(int expected, int id)
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            //Act
            var actualEntity = await repository.GetByIdAsync(id);
            var actual = actualEntity.ProductId;
            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public async Task OrderDetailRepository_GetAllAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            int expectedLength = 3;
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength, actual.Count());
        }


        [TestMethod]
        public async Task OrderDetailRepository_AddAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            int expectedLength = 4;
            var newEntity = new OrderDetail
            {
                Quantity = 4,
                ProductId = 1,
                OrderId = 1,
            };
            //Act
            await repository.AddAsync(newEntity);
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength, actual.Count());
        }

        [TestMethod]
        public async Task OrderDetailRepository_UpdateAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            decimal expectedProperty = 5;

            var newStatus = await repository.GetByIdAsync(1);
            newStatus.Quantity = expectedProperty;
            repository.Update(newStatus);
            //Act
            var actual = await repository.GetByIdAsync(newStatus.Id);
            //Assert
            Assert.AreEqual(expectedProperty, actual.Quantity);
        }

        [TestMethod]
        public async Task OrderDetailRepository_DeleteAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedLen = 2;

            var entitToDelete = await repository.GetByIdAsync(1);

            repository.Delete(entitToDelete);
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }

        [TestMethod]
        public async Task OrderDetailRepository_DeleteByIdAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedLen = 2;

            await repository.DeleteByIdAsync(3);
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }
    }
}
