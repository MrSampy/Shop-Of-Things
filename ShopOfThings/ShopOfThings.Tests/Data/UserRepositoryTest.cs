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

        [DataTestMethod]
        [DataRow("Test1 TestN", 1)]
        [DataRow("Test3 TestN4", 3)]
        public async Task UserRepository_GetByIdAsync(string expected, int id)
        {
            //Arrange
            var repository = await CreateRepositoryAsync();
            //Act
            var actualUser = await repository.GetByIdAsync(id);
            var actual = string.Format("{0} {1}", actualUser.Name, actualUser.SecondName);
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
                BirthDate = DateTime.Today,
                UserStatusId = 1
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

            var newStatus = await repository.GetByIdAsync(1);
            newStatus.Name = expectedName;

            repository.Update(newStatus);
            //Act
            var actual = await repository.GetByIdAsync(newStatus.Id);
            //Assert
            Assert.AreEqual(expectedName, actual.Name);
        }

        [TestMethod]
        public async Task UserRepository_DeleteAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            var expectedLen = 2;

            var entitToDelete = await repository.GetByIdAsync(3);

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
            var userStatusRepository = await CreateRepositoryAsync();

            var expectedLen = 2;

            await userStatusRepository.DeleteByIdAsync(2);
            //Act
            var actual = await userStatusRepository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }
        
    }
}
