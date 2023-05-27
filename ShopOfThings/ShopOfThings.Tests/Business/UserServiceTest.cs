using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ShopOfThings.Tests.UnitTestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopOfThings.Tests.Business
{
    [TestClass]
    public class UserServiceTest
    {
        private async Task<UserService> CreateService() 
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
               .EnableSensitiveDataLogging()
               .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new UserService(UnitTestHelper.CreateUnitOfWork(context), UnitTestHelper.CreateMapper());
        }

        [TestMethod]
        public async Task UserService_GetAllAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 3;
            //Act
            var users = await service.GetAllAsync();
            var actual = users.Count();
            //Assert
            Assert.AreEqual(expected,actual);
        }

        [TestMethod]
        public async Task UserService_GetAllUserRoleAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 2;
            //Act
            var userRoles = await service.GetAllUserRolesAsync();
            var actual = userRoles.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task UserService_GetByIdAsync()
        {
            //Arrange
            var service = await CreateService();
            const string expected = "Nick";
            //Act
            var users = await service.GetAllAsync();
            var userId = users.Last().Id;
            var user = await service.GetByIdAsync(userId);
            var actual = user.NickName;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task UserService_GetByIdAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.GetByIdAsync(userId), "User not found!");
        }

        [DataTestMethod]
        [DataRow("14523",true)]
        [DataRow("123", false)]
        public async Task UserService_VerifyPassword(string password, bool expected) 
        {
            //Arrange
            var service = await CreateService();
            //Act
            var users = await service.GetAllAsync();
            var userId = users.Last().Id;
            var actual = await service.VerifyPassword(userId,password);
            //Assert
            Assert.AreEqual(expected,actual);
        }

        [TestMethod]
        public async Task UserService_VerifyPassword_NotFoundException()
        {
            //Arrange
            var service = await CreateService();            
            //Act
            var userId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.VerifyPassword(userId,string.Empty), "User not found!");
        }

        [TestMethod]
        public async Task UserService_DeleteUserRoleAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 1;
            //Act
            var userRoleId = service.GetAllUserRolesAsync().Result.First().Id;
            await service.DeleteUserRoleAsync(userRoleId);
            var actual = service.GetAllUserRolesAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task UserService_DeleteUserRoleAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userRoleId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.DeleteUserRoleAsync(userRoleId), "User role not found!");
        }

        [TestMethod]
        public async Task UserService_DeleteAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 2;
            //Act
            var userRoleId = service.GetAllAsync().Result.First().Id;
            await service.DeleteAsync(userRoleId);
            var actual = service.GetAllAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task UserService_DeleteAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.DeleteAsync(userId), "User not found!");
        }

        [TestMethod]
        public async Task UserService_AddAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 4;
            //Act
            var userRole = service.GetAllUserRolesAsync().Result.First();
            var userModel = new UserModel
            {
                NickName = "N",
                Name = "test",
                SecondName = "seconf",
                Email = "tests@mail.com",
                BirthDate = DateTime.Now.AddYears(-20),
                Password = "123",
                UserRoleId = userRole.Id,
                UserRoleName = userRole.UserRoleName
            };
            await service.AddAsync(userModel);
            var actual = service.GetAllAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("","1234","1234","1234","1234")]
        [DataRow("14523", "", "1234", "1234", "1234")]
        [DataRow("14523", "1234", "", "1234", "1234")]
        [DataRow("14523", "1234", "1234", "", "1234")]
        [DataRow("14523", "1234", "1234", "1234", "")]
        public async Task UserService_AddAsync_WrongDataException(string nickName, string name, string secondName, string email, string password)
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userRole = service.GetAllUserRolesAsync().Result.First();
            var userModel = new UserModel
            {
                NickName = nickName,
                Name = name,
                SecondName = secondName,
                Email = email,
                BirthDate = DateTime.Now.AddYears(-20),
                Password = password,
                UserRoleId = userRole.Id,
                UserRoleName = userRole.UserRoleName
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(userModel), "Wrong data for user!");
        }


        [TestMethod]
        public async Task UserService_AddAsync_WrongBirthDateException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userRole = service.GetAllUserRolesAsync().Result.First();
            var userModel = new UserModel
            {
                NickName = "N",
                Name = "test",
                SecondName = "seconf",
                Email = "tests@mail.com",
                Password = "123",
                BirthDate = DateTime.Today.AddYears(-200),
                UserRoleId = userRole.Id,
                UserRoleName = userRole.UserRoleName
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(userModel), "Wrong birth date for user!");
        }

        [TestMethod]
        public async Task UserService_AddAsync_WrongBirthDateNullException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userRole = service.GetAllUserRolesAsync().Result.First();
            var userModel = new UserModel
            {
                NickName = "N",
                Name = "test",
                SecondName = "seconf",
                Email = "tests@mail.com",
                Password = "123",
                UserRoleId = userRole.Id,
                UserRoleName = userRole.UserRoleName
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(userModel), "Wrong birth date for user!");
        }

        [DataTestMethod]
        [DataRow("Nick", "14523")]
        [DataRow("14523", "123@test.com")]
        public async Task UserService_AddAsync_AlreadyExistsException(string nickName, string email)
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userRole = service.GetAllUserRolesAsync().Result.First();
            var userModel = new UserModel
            {
                NickName = nickName,
                Name = "test",
                SecondName = "seconf",
                Email = email,
                BirthDate = DateTime.Now.AddYears(-20),
                Password = "123",
                UserRoleId = userRole.Id,
                UserRoleName = userRole.UserRoleName
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(userModel), "User with such nickname or email already exists!");
        }

        [TestMethod]
        public async Task UserService_AddAsync_IncorrectEmailException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userRole = service.GetAllUserRolesAsync().Result.First();
            var userModel = new UserModel
            {
                NickName = "N",
                Name = "test",
                SecondName = "seconf",
                Email = "testsmail.com",
                BirthDate = DateTime.Now.AddYears(-20),
                Password = "123",
                UserRoleId = userRole.Id,
                UserRoleName = userRole.UserRoleName
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(userModel), "Incorrect email address!");
        }

        [TestMethod]
        public async Task UserService_AddAsync_RoleNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userRole = service.GetAllUserRolesAsync().Result.First();
            var userModel = new UserModel
            {
                NickName = "N",
                Name = "test",
                SecondName = "seconf",
                Email = "testsmail.com",
                BirthDate = DateTime.Now.AddYears(-20),
                Password = "123",
                UserRoleId = UnitTestHelper.GetWrongId(),
                UserRoleName = userRole.UserRoleName
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(userModel), "User role not found!");
        }

        [TestMethod]
        public async Task UserService_UpdateAsync()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var expected = service.GetAllAsync().Result.Last();
            var userRole = service.GetAllUserRolesAsync().Result.First();
            expected.NickName = "New";
            expected.Name = "New";
            expected.SecondName = "New";
            expected.Email = "tests@mail.com";
            expected.BirthDate = DateTime.Today.AddYears(-21);
            expected.UserRoleId = userRole.Id;
            expected.UserRoleName = userRole.UserRoleName;
            expected.Password = "321";
            await service.UpdateAsync(expected);
            var actual = await service.GetByIdAsync(expected.Id);
            expected.Password = actual.Password;
            //Assert
            var isEqual = expected.Name == actual.Name && expected.SecondName == actual.SecondName
                 && expected.NickName == actual.NickName && expected.Email == actual.Email
                  && expected.BirthDate == actual.BirthDate && expected.UserRoleName == actual.UserRoleName
                   && expected.UserRoleId == actual.UserRoleId && expected.Password == actual.Password;
            Assert.IsTrue(isEqual);
        }

        [DataTestMethod]
        [DataRow("14523")]
        [DataRow(null)]
        public async Task UserService_UpdateAsync_UserRoleNameNotFoundException(string? userRoleName)
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModel = service.GetAllAsync().Result.Last();
            var userRole = service.GetAllUserRolesAsync().Result.First();
            userModel.NickName = "New";
            userModel.Name = "New";
            userModel.SecondName = "New";
            userModel.Email = "tests@mail.com";
            userModel.BirthDate = DateTime.Today.AddYears(-21);
            userModel.UserRoleId = userRole.Id;
            userModel.UserRoleName = userRoleName;
            userModel.Password = "321";
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(userModel), "User role name not found!");
        }

        [DataTestMethod]
        [DataRow("", "1234", "1234", "1234", "1234")]
        [DataRow("14523", "", "1234", "1234", "1234")]
        [DataRow("14523", "1234", "", "1234", "1234")]
        [DataRow("14523", "1234", "1234", "", "1234")]
        [DataRow("14523", "1234", "1234", "1234", "")]
        public async Task UserService_UpdateAsync_WrongDataException(string nickName, string name, string secondName, string email, string password)
        {
            var service = await CreateService();
            //Act
            var userModel = service.GetAllAsync().Result.Last();
            var userRole = service.GetAllUserRolesAsync().Result.First();
            userModel.NickName = nickName;
            userModel.Name = name;
            userModel.SecondName = secondName;
            userModel.Email = email;
            userModel.BirthDate = DateTime.Today.AddYears(-21);
            userModel.UserRoleId = userRole.Id;
            userModel.UserRoleName = userRole.UserRoleName;
            userModel.Password = password;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(userModel), "Wrong data for user!");
        }

        [TestMethod]
        public async Task UserService_UpdateAsync_WrongBirthDateException()
        {
            var service = await CreateService();
            //Act
            var userModel = service.GetAllAsync().Result.Last();
            var userRole = service.GetAllUserRolesAsync().Result.First();
            userModel.NickName = "New";
            userModel.Name = "New";
            userModel.SecondName = "New";
            userModel.Email = "tests@mail.com";
            userModel.BirthDate = DateTime.Today.AddYears(-210);
            userModel.UserRoleId = userRole.Id;
            userModel.UserRoleName = userRole.UserRoleName;
            userModel.Password = "321";
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(userModel), "Wrong birth date for user!");
        }

        [DataTestMethod]
        [DataRow("123", "14523")]
        [DataRow("14523", "123@test.com")]
        public async Task UserService_UpdateAsync_AlreadyExistsException(string nickName, string email)
        {
            var service = await CreateService();
            //Act
            var userModel = service.GetAllAsync().Result.Last();
            var userRole = service.GetAllUserRolesAsync().Result.First();
            userModel.NickName = nickName;
            userModel.Name = "New";
            userModel.SecondName = "New";
            userModel.Email = email;
            userModel.BirthDate = DateTime.Today.AddYears(-21);
            userModel.UserRoleId = userRole.Id;
            userModel.UserRoleName = userRole.UserRoleName;
            userModel.Password = "321";
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(userModel), "User with such nickname or email already exists!");
        }

        [TestMethod]
        public async Task UserService_UpdateAsync_IncorrectEmailException()
        {
            var service = await CreateService();
            //Act
            var userModel = service.GetAllAsync().Result.Last();
            var userRole = service.GetAllUserRolesAsync().Result.First();
            userModel.NickName = "New";
            userModel.Name = "New";
            userModel.SecondName = "New";
            userModel.Email = "testsmail.com";
            userModel.BirthDate = DateTime.Today.AddYears(-21);
            userModel.UserRoleId = userRole.Id;
            userModel.UserRoleName = userRole.UserRoleName;
            userModel.Password = "321";
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(userModel), "Incorrect email address!");
        }

        [TestMethod]
        public async Task UserService_UpdateAsync_RoleNotFoundException()
        {
            var service = await CreateService();
            //Act
            var userModel = service.GetAllAsync().Result.First();
            var userRole = service.GetAllUserRolesAsync().Result.Last();
            userModel.NickName = "New";
            userModel.Name = "New";
            userModel.SecondName = "New";
            userModel.Email = "tests@mail.com";
            userModel.BirthDate = DateTime.Today.AddYears(-21);
            userModel.UserRoleId = userRole.Id;
            userModel.Password = "321";
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(userModel), "User role not found!");
        }
        [TestMethod]
        public async Task UserService_UpdateAsync_UserNotFoundException()
        {
            var service = await CreateService();
            //Act
            var userModel = service.GetAllAsync().Result.First();
            var userRole = service.GetAllUserRolesAsync().Result.Last();
            userModel.NickName = "New";
            userModel.Name = "New";
            userModel.SecondName = "New";
            userModel.Email = "tests@mail.com";
            userModel.BirthDate = DateTime.Today.AddYears(-21);
            userModel.UserRoleId = userRole.Id;
            userModel.UserRoleName = userRole.UserRoleName;
            userModel.Password = "321";
            userModel.Id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(userModel), "User not found!");
        }

        [TestMethod]
        public async Task UserService_AddUserRoleAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 3;
            //Act
            var userRoleModel = new UserRoleModel
            {
                UserRoleName = "New"
            };
            await service.AddUserRoleAsync(userRoleModel);
            var actual = service.GetAllUserRolesAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task UserService_AddUserRoleAsync_WrongDataException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModel = new UserRoleModel
            {
                UserRoleName = ""
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddUserRoleAsync(userModel), "Wrong data for user role!");
        }


        [TestMethod]
        public async Task UserService_AddUserRoleAsync_AlreadyExistsException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModel = new UserRoleModel
            {
                UserRoleName = "Customer"
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddUserRoleAsync(userModel), "User role with such name already exists!");
        }

        [TestMethod]
        public async Task UserService_UpdatUserRoleAsync()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var expected = service.GetAllUserRolesAsync().Result.First();
            expected.UserRoleName = "New";
            await service.UpdatUserRoleAsync(expected);
            var actual = service.GetAllUserRolesAsync().Result.First();
            //Assert
            Assert.AreEqual(expected.UserRoleName, actual.UserRoleName);
        }


        [TestMethod]
        public async Task UserService_UpdatUserRoleAsync_WrongDataException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModel = service.GetAllUserRolesAsync().Result.First();
            userModel.UserRoleName = "";
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdatUserRoleAsync(userModel), "Wrong data for user role!");
        }

        [TestMethod]
        public async Task UserService_UpdatUserRoleAsync_AlreadyExistsException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModel = service.GetAllUserRolesAsync().Result.First();
            userModel.UserRoleName = "Customer";
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdatUserRoleAsync(userModel), "User role with such name already exists!");
        }

        [TestMethod]
        public async Task UserService_UpdatUserRoleAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModel = service.GetAllUserRolesAsync().Result.First();
            userModel.UserRoleName = "123";
            userModel.Id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdatUserRoleAsync(userModel), "User role not found!");
        }

    }
}
