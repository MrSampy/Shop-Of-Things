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

        [DataTestMethod]
        [DataRow("R1 D1", 1)]
        [DataRow("R2 D2", 2)]
        public async Task ReceiptRepository_GetByIdAsync(string expected, int id)
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            //Act
            var actualEntity = await repository.GetByIdAsync(id);
            var actual = string.Format("{0} {1}", actualEntity.ReceiptName, actualEntity.ReceiptDescription);
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
               ReceiptDescription = string.Empty,
               UserId = 2,              
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

            var newEntity = await repository.GetByIdAsync(1);
            newEntity.ReceiptName = expectedName;

            repository.Update(newEntity);
            //Act
            var actual = await repository.GetByIdAsync(newEntity.Id);
            //Assert
            Assert.AreEqual(expectedName, actual.ReceiptName);
        }

        [TestMethod]
        public async Task ReceiptRepository_DeleteAsync()
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
        public async Task ReceiptRepository_DeleteByIdAsync()
        {
            //Arrange
            var userStatusRepository = await CreateRepositoryAsync();

            var expectedLen = 1;

            await userStatusRepository.DeleteByIdAsync(2);
            //Act
            var actual = await userStatusRepository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }
    }
}
