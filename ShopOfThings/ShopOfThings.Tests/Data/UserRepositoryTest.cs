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
    public class UserRepositoryTest
    {
        public async Task<UserRepository> CreateRepositoryAsync()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new UserRepository(context);

        }

        [TestMethod]
        public async Task UserRepository_GetByIdAsync()
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
        public async Task UserRepository_GetAllAsync()
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
        public async Task UserRepository_AddAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            int expectedLength = 4;
            var newEntity = new User
            {
                Name = string.Empty,
                SecondName = string.Empty,
                Email = string.Empty,
                Password = string.Empty,
                BirthDate = DateTime.Today
            };
            //Act
            await repository.AddAsync(newEntity);
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength, actual.Count());
        }

        [TestMethod]
        public async Task UserRepository_UpdateAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            string expectedName = "TestNew";

            //Act
            var entityToUpdate = repository.GetAllAsync().Result.Last();

            entityToUpdate.Name = expectedName;

            repository.Update(entityToUpdate);

            var actual = await repository.GetByIdAsync(entityToUpdate.Id);
            //Assert
            Assert.AreEqual(expectedName, actual.Name);
        }

        [TestMethod]
        public async Task UserRepository_DeleteAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedLen = 2;

            var entitToDelete = repository.GetAllAsync().Result.Last();

            repository.Delete(entitToDelete);
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }

        [TestMethod]
        public async Task UserRepository_DeleteByIdAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedLen = 2;

            var entitToDelete = repository.GetAllAsync().Result.First();

            await repository.DeleteByIdAsync(entitToDelete.Id);
            //Act
            var actual = await repository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }
        
    }
}
