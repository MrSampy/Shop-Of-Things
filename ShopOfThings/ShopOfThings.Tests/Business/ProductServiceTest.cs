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
    public class ProductServiceTest
    {
        private async Task<ProductService> CreateService()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
               .EnableSensitiveDataLogging()
               .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new ProductService(UnitTestHelper.CreateUnitOfWork(context), UnitTestHelper.CreateMapper());
        }

        [TestMethod]
        public async Task ProductService_GetAllAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 6;
            //Act
            var users = await service.GetAllAsync();
            var actual = users.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ProductService_GetAllStorageTypesAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 2;
            //Act
            var users = await service.GetAllStorageTypesAsync();
            var actual = users.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ProductService_GetAllProductCategoriesAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 2;
            //Act
            var users = await service.GetAllProductCategoriesAsync();
            var actual = users.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ProductService_GetByIdAsync()
        {
            //Arrange
            var service = await CreateService();
            const string expected = "ProductDescription6";
            //Act
            var users = await service.GetAllAsync();
            var userId = users.Last().Id;
            var user = await service.GetByIdAsync(userId);
            var actual = user.ProductDescription;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ProductService_GetByIdAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.GetByIdAsync(id), "Product not found!");
        }

        [TestMethod]
        public async Task ProductService_GetByIdAsync_GetByFilter()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 3;
            var storageTypeId = service.GetAllStorageTypesAsync().Result.First(x => x.StorageTypeName.Equals("kg")).Id;
            var productCategoryId = service.GetAllProductCategoriesAsync().Result.First(x => x.ProductCategoryName.Equals("Meat")).Id;
            var filter = new ProductFilterSearchModel
            {
                MinPrice = 9.1M,
                MaxPrice = 18.2M,
                StorageTypeId = storageTypeId,
                ProductCategoryId = productCategoryId
            };
            //Act
            var actual = service.GetByFilterAsync(filter).Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ProductService_DeleteAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 5;
            //Act
            var id = service.GetAllAsync().Result.First().Id;
            await service.DeleteAsync(id);
            var actual = service.GetAllAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ProductService_DeleteAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.DeleteAsync(id), "Product not found!");
        }

        [TestMethod]
        public async Task ProductService_DeleteStorageTypeAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 1;
            //Act
            var id = service.GetAllStorageTypesAsync().Result.First().Id;
            await service.DeleteStorageTypeAsync(id);
            var actual = service.GetAllStorageTypesAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ProductService_DeleteStorageTypeAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.DeleteStorageTypeAsync(id), "Storage type not found!");
        }

        [TestMethod]
        public async Task ProductService_DeleteProductCategoryAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 1;
            //Act
            var id = service.GetAllProductCategoriesAsync().Result.First().Id;
            await service.DeleteProductCategoryAsync(id);
            var actual = service.GetAllProductCategoriesAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ProductService_DeleteProductCategoryAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.DeleteProductCategoryAsync(id), "Product category not found!");
        }

        [TestMethod]
        public async Task ProductService_AddAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 7;
            //Act
            var storageType = service.GetAllStorageTypesAsync().Result.First();
            var productCategory = service.GetAllProductCategoriesAsync().Result.First();
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var productModel = new ProductModel
            {
                ProductName = "name",
                ProductDescription = "desc",
                Price = 10,
                Amount = 10,
                StorageTypeId = storageType.Id,
                StorageTypeName = storageType.StorageTypeName,
                ProductCategoryId = productCategory.Id,
                ProductCategoryName = productCategory.ProductCategoryName,
                UserId = userModelId
            };
            await service.AddAsync(productModel);
            var actual = service.GetAllAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }


        [DataTestMethod]
        [DataRow("", "1234", "12", "10")]
        [DataRow("1234", "", "12", "10")]
        [DataRow("1234", "1234", "-12", "10")]
        [DataRow("1234", "1234", "12", "-10")]
        public async Task ProductService_AddAsync_WrongData(string productName, string productDesc, string price, string amount)
        {
            //Arrange
            var service = await CreateService();
            //Act
            var storageType = service.GetAllStorageTypesAsync().Result.First();
            var productCategory = service.GetAllProductCategoriesAsync().Result.First();
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var productModel = new ProductModel
            {
                ProductName = productName,
                ProductDescription = productDesc,
                Price = Convert.ToDecimal(price),
                Amount = Convert.ToDecimal(amount),
                StorageTypeId = storageType.Id,
                StorageTypeName = storageType.StorageTypeName,
                ProductCategoryId = productCategory.Id,
                ProductCategoryName = productCategory.ProductCategoryName,
                UserId = userModelId
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(productModel), "Wrong data for product!");
        }

        [TestMethod]
        public async Task ProductService_AddAsync_UserNotFound()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var storageType = service.GetAllStorageTypesAsync().Result.First();
            var productCategory = service.GetAllProductCategoriesAsync().Result.First();
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var productModel = new ProductModel
            {
                ProductName = "name",
                ProductDescription = "desc",
                Price = 10,
                Amount = 10,
                StorageTypeId = storageType.Id,
                StorageTypeName = storageType.StorageTypeName,
                ProductCategoryId = productCategory.Id,
                ProductCategoryName = productCategory.ProductCategoryName,
                UserId = UnitTestHelper.GetWrongId()
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(productModel), "User not found!");
        }

        [TestMethod]
        public async Task ProductService_AddAsync_StorageNotFound()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var storageType = service.GetAllStorageTypesAsync().Result.First();
            var productCategory = service.GetAllProductCategoriesAsync().Result.First();
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var productModel = new ProductModel
            {
                ProductName = "name",
                ProductDescription = "desc",
                Price = 10,
                Amount = 10,
                StorageTypeId = UnitTestHelper.GetWrongId(),
                StorageTypeName = storageType.StorageTypeName,
                ProductCategoryId = productCategory.Id,
                ProductCategoryName = productCategory.ProductCategoryName,
                UserId = userModelId
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(productModel), "Storage type not found!");
        }

        [TestMethod]
        public async Task ProductService_AddAsync_CategoryNotFound()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var storageType = service.GetAllStorageTypesAsync().Result.First();
            var productCategory = service.GetAllProductCategoriesAsync().Result.First();
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var productModel = new ProductModel
            {
                ProductName = "name",
                ProductDescription = "desc",
                Price = 10,
                Amount = 10,
                StorageTypeId = storageType.Id,
                StorageTypeName = storageType.StorageTypeName,
                ProductCategoryId = UnitTestHelper.GetWrongId(),
                ProductCategoryName = productCategory.ProductCategoryName,
                UserId = userModelId
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(productModel), "Product category not found!");
        }

        [TestMethod]
        public async Task ProductService_UpdateAsync()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var storageType = service.GetAllStorageTypesAsync().Result.First();
            var productCategory = service.GetAllProductCategoriesAsync().Result.First();
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var productModel = service.GetAllAsync().Result.First();
            productModel.ProductName = "name";
            productModel.ProductDescription = "desc";
            productModel.Price = 10;
            productModel.Amount = 10;
            productModel.StorageTypeId = storageType.Id;
            productModel.StorageTypeName = storageType.StorageTypeName;
            productModel.ProductCategoryId = productCategory.Id;
            productModel.ProductCategoryName = productCategory.ProductCategoryName;
            productModel.UserId = userModelId;
            await service.UpdateAsync(productModel);
            var actual = service.GetAllAsync().Result.First();
            var isEqual = productModel.ProductName == actual.ProductName && productModel.ProductDescription == actual.ProductDescription
                && productModel.Price == actual.Price && productModel.Amount == actual.Amount && productModel.StorageTypeId == actual.StorageTypeId
                && productModel.StorageTypeName == actual.StorageTypeName && productModel.ProductCategoryId == actual.ProductCategoryId
                && productModel.ProductCategoryName == actual.ProductCategoryName;
            //Assert
            Assert.IsTrue(isEqual);
        }

        [DataTestMethod]
        [DataRow("", "1234", "12", "10")]
        [DataRow("1234", "", "12", "10")]
        [DataRow("1234", "1234", "-12", "10")]
        [DataRow("1234", "1234", "12", "-10")]
        public async Task ProductService_UpdateAsync_WrongData(string productName, string productDesc, string price, string amount)
        {
            //Arrange
            var service = await CreateService();
            //Act
            var storageType = service.GetAllStorageTypesAsync().Result.First();
            var productCategory = service.GetAllProductCategoriesAsync().Result.First();
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var productModel = service.GetAllAsync().Result.First();
            productModel.ProductName = productName;
            productModel.ProductDescription = productDesc;
            productModel.Price = Convert.ToDecimal(price);
            productModel.Amount = Convert.ToDecimal(amount);
            productModel.StorageTypeId = storageType.Id;
            productModel.StorageTypeName = storageType.StorageTypeName;
            productModel.ProductCategoryId = productCategory.Id;
            productModel.ProductCategoryName = productCategory.ProductCategoryName;
            productModel.UserId = userModelId;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(productModel), "Wrong data for product!");
        }

        [TestMethod]
        public async Task ProductService_UpdateAsync_NotFound()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var storageType = service.GetAllStorageTypesAsync().Result.First();
            var productCategory = service.GetAllProductCategoriesAsync().Result.First();
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var productModel = service.GetAllAsync().Result.First();
            productModel.Id = UnitTestHelper.GetWrongId();
            productModel.ProductName = "name";
            productModel.ProductDescription = "desc";
            productModel.Price = 10;
            productModel.Amount = 10;
            productModel.StorageTypeId = storageType.Id;
            productModel.StorageTypeName = storageType.StorageTypeName;
            productModel.ProductCategoryId = productCategory.Id;
            productModel.ProductCategoryName = productCategory.ProductCategoryName;
            productModel.UserId = userModelId;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(productModel), "Product not found!");
        }

        [TestMethod]
        public async Task ProductService_UpdateAsync_UserNotFound()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var storageType = service.GetAllStorageTypesAsync().Result.First();
            var productCategory = service.GetAllProductCategoriesAsync().Result.First();
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var productModel = service.GetAllAsync().Result.First();
            productModel.ProductName = "name";
            productModel.ProductDescription = "desc";
            productModel.Price = 10;
            productModel.Amount = 10;
            productModel.StorageTypeId = storageType.Id;
            productModel.StorageTypeName = storageType.StorageTypeName;
            productModel.ProductCategoryId = productCategory.Id;
            productModel.ProductCategoryName = productCategory.ProductCategoryName;
            productModel.UserId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(productModel), "User not found!");
        }

        [TestMethod]
        public async Task ProductService_UpdateAsync_StorageNotFound()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var storageType = service.GetAllStorageTypesAsync().Result.First();
            var productCategory = service.GetAllProductCategoriesAsync().Result.First();
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var productModel = service.GetAllAsync().Result.First();
            productModel.ProductName = "name";
            productModel.ProductDescription = "desc";
            productModel.Price = 10;
            productModel.Amount = 10;
            productModel.StorageTypeId = UnitTestHelper.GetWrongId();
            productModel.StorageTypeName = storageType.StorageTypeName;
            productModel.ProductCategoryId = productCategory.Id;
            productModel.ProductCategoryName = productCategory.ProductCategoryName;
            productModel.UserId = userModelId;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(productModel), "Storage type not found!");
        }

        [TestMethod]
        public async Task ProductService_UpdateAsync_CategoryNotFound()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var storageType = service.GetAllStorageTypesAsync().Result.First();
            var productCategory = service.GetAllProductCategoriesAsync().Result.First();
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var productModel = service.GetAllAsync().Result.First();
            productModel.ProductName = "name";
            productModel.ProductDescription = "desc";
            productModel.Price = 10;
            productModel.Amount = 10;
            productModel.StorageTypeId = storageType.Id;
            productModel.StorageTypeName = storageType.StorageTypeName;
            productModel.ProductCategoryId = UnitTestHelper.GetWrongId();
            productModel.ProductCategoryName = productCategory.ProductCategoryName;
            productModel.UserId = userModelId;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(productModel), "Product category not found!");
        }
    }
}
