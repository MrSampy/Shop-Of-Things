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

        [DataTestMethod]
        [DataRow("Admin",1)]
        [DataRow("Customer", 2)]
        public async Task UserStatusRepository_GetByIdAsync(string expected,int id) 
        {
            //Arrange
            UserStatusRepository userStatusRepository = await CreateRepositoryAsync();
            //Act
            var actual = await userStatusRepository.GetByIdAsync(id);
            //Assert
            Assert.AreEqual(expected,actual.UserStatusName);

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
            var userStatusRepository = await CreateRepositoryAsync();

            string expectedStatusName = "New";

            UserStatus newStatus = new() { Id = 1, UserStatusName = expectedStatusName };

            userStatusRepository.Update(newStatus);
            //Act
            var actual = await userStatusRepository.GetByIdAsync(newStatus.Id);
            //Assert
            Assert.AreEqual(expectedStatusName, actual.UserStatusName);
        }

        [TestMethod]
        public async Task UserStatusRepository_DeleteAsync()
        {
            //Arrange
            var userStatusRepository = await CreateRepositoryAsync();

            var expectedLen = 1;

            var entitToDelete = await userStatusRepository.GetByIdAsync(1);

            userStatusRepository.Delete(entitToDelete);
            //Act
            var actual = await userStatusRepository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }

        [TestMethod]
        public async Task UserStatusRepository_DeleteByIdAsync()
        {
            //Arrange
            var userStatusRepository = await CreateRepositoryAsync();

            var expectedLen = 1;

            await userStatusRepository.DeleteByIdAsync(1);
            //Act
            var actual = await userStatusRepository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLen, actual.Count());
        }
    }
}
