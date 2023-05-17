using Data.Data;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopOfThings.Tests.UnitTestHelpers;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;


namespace ShopOfThings.Tests.Data
{
    [TestClass]
    public class UserStatusRepositoryTest
    {

        public async Task<UserStatusRepository> CreateRepositoryAsync()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new UserStatusRepository(context);

        }

        [TestMethod]
        public async Task UserStatusRepository_GetByIdAsync() 
        {
            //Arrange
            UserStatusRepository repository = await CreateRepositoryAsync();
            //Act
            var expected = repository.GetAllAsync().Result.First();
            var actual = await repository.GetByIdAsync(expected.Id);
            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public async Task UserStatusRepository_GetAllAsync() 
        {
            //Arrange
            UserStatusRepository userStatusRepository = await CreateRepositoryAsync();
            int expectedLength = 2;
            //Act
            var actual = await userStatusRepository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength,actual.Count());
        }


        [TestMethod]
        public async Task UserStatusRepository_AddAsync()
        {
            //Arrange
            UserStatusRepository userStatusRepository = await CreateRepositoryAsync();
            int expectedLength = 3;
            UserStatus newStartus = new UserStatus { UserStatusName = "New"};
            //Act
            await userStatusRepository.AddAsync(newStartus);
            var actual = await userStatusRepository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength, actual.Count());
        }
        
        [TestMethod]
        public async Task UserStatusRepository_UpdateAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            string expectedStatusName = "New";
            //Act
            var entityToUpdate = repository.GetAllAsync().Result.Last();

            entityToUpdate.UserStatusName = expectedStatusName;

            repository.Update(entityToUpdate);

            var actual = await repository.GetByIdAsync(entityToUpdate.Id);
            //Assert
            Assert.AreEqual(expectedStatusName, actual.UserStatusName);
        }

        [TestMethod]
        public async Task UserStatusRepository_DeleteAsync()
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
        public async Task UserStatusRepository_DeleteByIdAsync()
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
