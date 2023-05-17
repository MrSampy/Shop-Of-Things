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
    public class ReceiptRepositoryTest
    {
        public async Task<ReceiptRepository> CreateRepositoryAsync()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new ReceiptRepository(context);

        }

        [TestMethod]
        public async Task ReceiptRepository_GetByIdAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            //Act
            var expected = repository.GetAllAsync().Result.First();
            var actual = await repository.GetByIdAsync(expected.Id);
            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public async Task ReceiptRepository_GetAllAsync()
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
        public async Task ReceiptRepository_AddAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            int expectedLength = 3;
            var newEntity = new Receipt
            {
               ReceiptName = string.Empty,
               ReceiptDescription = string.Empty
            };
            //Act
            await repository.AddAsync(newEntity);
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength, actual.Count());
        }

        [TestMethod]
        public async Task ReceiptRepository_UpdateAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            string expectedName = "TestNew";

            //Act
            var entityToUpdate = repository.GetAllAsync().Result.Last();

            entityToUpdate.ReceiptName = expectedName;

            repository.Update(entityToUpdate);

            var actual = await repository.GetByIdAsync(entityToUpdate.Id);
            //Assert
            Assert.AreEqual(expectedName, actual.ReceiptName);
        }

        [TestMethod]
        public async Task ReceiptRepository_DeleteAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedLen = 1;

            var entitToDelete = repository.GetAllAsync().Result.Last();

            repository.Delete(entitToDelete);
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }

        [TestMethod]
        public async Task ReceiptRepository_DeleteByIdAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedLen = 1;

            var entitToDelete = repository.GetAllAsync().Result.First();

            await repository.DeleteByIdAsync(entitToDelete.Id);
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }
    }
}
