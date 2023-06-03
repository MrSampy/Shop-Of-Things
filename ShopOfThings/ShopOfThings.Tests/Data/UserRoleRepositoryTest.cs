using Data.Data;
using Data.Repositories;
using ShopOfThings.Tests.UnitTestHelpers;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ShopOfThings.Tests.Data
{
    [TestClass]
    public class UserRoleRepositoryTest
    {

        public async Task<UserRoleRepository> CreateRepositoryAsync()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new UserRoleRepository(context);

        }

        [TestMethod]
        public async Task UserRoleRepository_GetByIdAsync() 
        {
            //Arrange
            UserRoleRepository repository = await CreateRepositoryAsync();
            //Act
            var expected = repository.GetAllAsync().Result.First();
            var actual = await repository.GetByIdAsync(expected.Id);
            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public async Task UserRoleRepository_GetAllAsync() 
        {
            //Arrange
            UserRoleRepository userStatusRepository = await CreateRepositoryAsync();
            int expectedLength = 2;
            //Act
            var actual = await userStatusRepository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength,actual.Count());
        }


        [TestMethod]
        public async Task UserRoleRepository_AddAsync()
        {
            //Arrange
            UserRoleRepository userRoleRepository = await CreateRepositoryAsync();
            int expectedLength = 3;
            UserRole newStartus = new UserRole { UserRoleName = "New"};
            //Act
            await userRoleRepository.AddAsync(newStartus);
            var actual = await userRoleRepository.GetAllAsync();
            //Assert
            Assert.AreEqual(expectedLength, actual.Count());
        }
        
        [TestMethod]
        public async Task UserRoleRepository_UpdateAsync()
        {
            //Arrange
            var repository = await CreateRepositoryAsync();

            string expectedStatusName = "New";
            //Act
            var entityToUpdate = repository.GetAllAsync().Result.Last();

            entityToUpdate.UserRoleName = expectedStatusName;

            repository.Update(entityToUpdate);

            var actual = await repository.GetByIdAsync(entityToUpdate.Id);
            //Assert
            Assert.AreEqual(expectedStatusName, actual.UserRoleName);
        }

        [TestMethod]
        public async Task UserRoleRepository_DeleteAsync()
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
        public async Task UserRoleRepository_DeleteByIdAsync()
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
