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
            UserStatusRepository userStatusRepository = await CreateRepositoryAsync();

            var actual = await userStatusRepository.GetByIdAsync(id);

            Assert.AreEqual(expected,actual.UserStatusName);

        }

        [TestMethod]
        public async Task UserStatusRepository_GetAllAsync() 
        {
            UserStatusRepository userStatusRepository = await CreateRepositoryAsync();

            int expectedLength = 2;

            var actual = await userStatusRepository.GetAllAsync();

            Assert.AreEqual(expectedLength,actual.Count());
        }


        [TestMethod]
        public async Task UserStatusRepository_AddAsync()
        {
            UserStatusRepository userStatusRepository = await CreateRepositoryAsync();
            int expectedLength = 3;
            UserStatus newStartus = new UserStatus { UserStatusName = "New"};

            await userStatusRepository.AddAsync(newStartus);
            var actual = await userStatusRepository.GetAllAsync();

            Assert.AreEqual(expectedLength, actual.Count());
        }
        
        [TestMethod]
        public async Task UserStatusRepository_UpdateAsync()
        {
            var userStatusRepository = await CreateRepositoryAsync();
            string expectedStatusName = "New";
            UserStatus newStatus = new() { Id = 1, UserStatusName = expectedStatusName };

            userStatusRepository.Update(newStatus);

            var actual = await userStatusRepository.GetByIdAsync(newStatus.Id);

            Assert.AreEqual(expectedStatusName, actual.UserStatusName);
        }

        [TestMethod]
        public async Task UserStatusRepository_DeleteAsync()
        {
            var userStatusRepository = await CreateRepositoryAsync();
            var expectedLen = 1;
            var entitToDelete = await userStatusRepository.GetByIdAsync(1);

            userStatusRepository.Delete(entitToDelete);

            var actual = await userStatusRepository.GetAllAsync();

            Assert.AreEqual(expectedLen, actual.Count());
        }

        [TestMethod]
        public async Task UserStatusRepository_DeleteByIdAsync()
        {
            var userStatusRepository = await CreateRepositoryAsync();
            var expectedLen = 1;

            await userStatusRepository.DeleteByIdAsync(1);

            var actual = await userStatusRepository.GetAllAsync();

            Assert.AreEqual(expectedLen, actual.Count());
        }
    }
}
