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
    public class ProductRepositoryTest
    {
        public async Task<ProductRepository> CreateRepositoryAsync()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new ProductRepository(context);

        }

        [DataTestMethod]
        [DataRow("Produict1 ProductDescription1", 1)]
        [DataRow("Produict3 ProductDescription3", 3)]
        public async Task ProductRepository_GetByIdAsync(string expected, int id)
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            //Act
            var actualEntity = await repository.GetByIdAsync(id);
            var actual = string.Format("{0} {1}", actualEntity.ProductName,actualEntity.ProductDescription);
            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public async Task ProductRepository_GetAllAsync()
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
        public async Task ProductRepository_AddAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            int expectedLength = 4;
            var newEntity = new Product
            {
                ProductName = string.Empty,
                ProductDescription = string.Empty,
                Price = 1,
                Amount = 4,
                UserId = 1,
                StorageTypeId = 2
            };
            //Act
            await repository.AddAsync(newEntity);
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength, actual.Count());
        }

        [TestMethod]
        public async Task ProductRepository_UpdateAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            string expectedProperty = "New";

            var newStatus = await repository.GetByIdAsync(1);
            newStatus.ProductName = expectedProperty;
            repository.Update(newStatus);
            //Act
            var actual = await repository.GetByIdAsync(newStatus.Id);
            //Assert
            Assert.AreEqual(expectedProperty, actual.ProductName);
        }

        [TestMethod]
        public async Task ProductRepository_DeleteAsync()
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
        public async Task ProductRepository_DeleteByIdAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedLen = 2;

            await repository.DeleteByIdAsync(1);
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }
    }
}
