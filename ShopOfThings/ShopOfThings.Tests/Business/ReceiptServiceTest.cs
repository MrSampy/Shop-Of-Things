using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using ShopOfThings.Tests.UnitTestHelpers;

namespace ShopOfThings.Tests.Business
{
    [TestClass]
    public class ReceiptServiceTest
    {
        private async Task<ReceiptService> CreateService()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
               .EnableSensitiveDataLogging()
               .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new ReceiptService(UnitTestHelper.CreateUnitOfWork(context), UnitTestHelper.CreateMapper());
        }

        [TestMethod]
        public async Task ReceiptService_GetAllAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 8;
            //Act
            var receipts = await service.GetAllAsync();
            var actual = receipts.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public async Task ReceiptService_GetByIdAsync()
        {
            //Arrange
            var service = await CreateService();
            var expected = service.GetAllAsync().Result.First().ReceiptName;
            //Act
            var receipts = await service.GetAllAsync();
            var receiptId = receipts.First().Id;
            var receipt = await service.GetByIdAsync(receiptId);
            var actual = receipt.ReceiptName;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ReceiptService_GetByIdAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.GetByIdAsync(id), "Receipt not found!");
        }

        [TestMethod]
        public async Task ReceiptService_GetReceiptDetailsAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 2;
            //Act
            var receipts = await service.GetAllAsync();
            var receiptId = receipts.First().Id;
            var receiptDetails = await service.GetReceiptDetailsAsync(receiptId);
            var actual = receiptDetails.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ReceiptService_GetReceiptDetailsAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.GetReceiptDetailsAsync(id), "Receipt not found!");
        }

        [TestMethod]
        public async Task ReceiptService_DeleteAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 7;
            //Act
            var id = service.GetAllAsync().Result.First().Id;
            await service.DeleteAsync(id);
            var actual = service.GetAllAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ReceiptService_DeleteAsync_ReceiptNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.DeleteAsync(id), "Receipt not found!");
        }

        [TestMethod]
        public async Task ReceiptService_UpdateAsync()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var receiptModel = service.GetAllAsync().Result.First();
            receiptModel.ReceiptName = "123";
            receiptModel.UserId = userModelId;
            receiptModel.ReceiptDescription = "12343";
            await service.UpdateAsync(receiptModel);
            var actual = service.GetAllAsync().Result.First();
            var isEqual = receiptModel.UserId == actual.UserId && receiptModel.ReceiptName == actual.ReceiptName
                && receiptModel.ReceiptDescription == actual.ReceiptDescription;
            //Assert
            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        public async Task ReceiptService_UpdateAsync_UserNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = UnitTestHelper.GetWrongId();
            var receiptModel = service.GetAllAsync().Result.First();
            receiptModel.ReceiptName = "123";
            receiptModel.UserId = userModelId;
            receiptModel.ReceiptDescription = "12343";
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(receiptModel), "User not found!");
        }

        [TestMethod]
        public async Task ReceiptService_UpdateAsync_ReceiptNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var receiptModel = service.GetAllAsync().Result.First();
            receiptModel.Id = UnitTestHelper.GetWrongId();
            receiptModel.ReceiptName = "123";
            receiptModel.UserId = userModelId;
            receiptModel.ReceiptDescription = "12343";
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(receiptModel), "Receipt not found!");
        }

        [DataTestMethod]
        [DataRow("", "14523")]
        [DataRow("123", "")]
        public async Task ReceiptService_UpdateAsync_WrongDataException(string receiptName, string receiptDescription)
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var receiptModel = service.GetAllAsync().Result.First();
            receiptModel.ReceiptName = receiptName;
            receiptModel.UserId = userModelId;
            receiptModel.ReceiptDescription = receiptDescription;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(receiptModel), "Wrong data for receipt!");
        }

        [TestMethod]
        public async Task ReceiptService_AddAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 9;
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var receiptModel = new ReceiptModel
            {
                UserId = userModelId,
                ReceiptName = "123",
                ReceiptDescription = "12343"
            };
            await service.AddAsync(receiptModel);
            var actual = service.GetAllAsync().Result.Count();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ReceiptService_AddAsync_UserNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = UnitTestHelper.GetWrongId();
            var receiptModel = new ReceiptModel
            {
                UserId = userModelId,
                ReceiptName = "123",
                ReceiptDescription = "12343"
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(receiptModel), "User not found!");
        }

        [DataTestMethod]
        [DataRow("", "14523")]
        [DataRow("123", "")]
        public async Task ReceiptService_AddAsync_WrongDataException(string receiptName, string receiptDescription)
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var receiptModel = new ReceiptModel
            {
                UserId = userModelId,
                ReceiptName = receiptName,
                ReceiptDescription = receiptDescription
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(receiptModel), "Wrong data for receipt!");
        }

        [TestMethod]
        public async Task ReceiptService_RemoveProductByIdAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expectedOrderDetailsCount = 1;
            //Act
            var receiptModelId = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            await service.RemoveProductByIdAsync(productId, receiptModelId);
            var actual = service.GetAllAsync().Result.First().ReceiptDetailsIds.Count();
            //Assert
            Assert.AreEqual(expectedOrderDetailsCount, actual);
        }

        [TestMethod]
        public async Task ReceiptService_RemoveProductByIdAsync_NoChanges()
        {
            //Arrange
            var service = await CreateService();
            const int expectedOrderDetailsCount = 2;
            //Act
            var receiptModelId = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.Last().Id;
            await service.RemoveProductByIdAsync(productId, receiptModelId);
            var actual = service.GetAllAsync().Result.First().ReceiptDetailsIds.Count();
            //Assert
            Assert.AreEqual(expectedOrderDetailsCount, actual);
        }

        [TestMethod]
        public async Task ReceiptService_RemoveProductByIdAsync_ReceiptNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var receiptModelId = UnitTestHelper.GetWrongId();
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.Last().Id;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.RemoveProductByIdAsync(productId, receiptModelId), "Receipt not found!");
        }

        [TestMethod]
        public async Task ReceiptService_RemoveProductByIdAsync_ProductNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var receiptModelId = service.GetAllAsync().Result.First().Id;
            var productId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.RemoveProductByIdAsync(productId, receiptModelId), "Product not found!");
        }

        [TestMethod]
        public async Task ReceiptService_AddProductAsync_AddReceiptDetail()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 3;
            //Act
            var receiptModelId = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.Last().Id;
            decimal quantity = 1;
            await service.AddProductAsync(receiptModelId, productId, quantity);
            var actual = service.GetByIdAsync(receiptModelId).Result.ReceiptDetailsIds.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ReceiptService_AddProductAsync_AddProductQuantity()
        {
            //Arrange
            var service = await CreateService();
            const int expectedReceiptDetailsCount = 2;
            //Act
            var receiptModelId = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = 1;
            var expectedProductQuantity = service.GetReceiptDetailsAsync(receiptModelId).Result.First(x => x.ProductId.Equals(productId)).Amount + quantity;
            await service.AddProductAsync(receiptModelId, productId, quantity);
            var actualReceiptDetailsCount = service.GetByIdAsync(receiptModelId).Result.ReceiptDetailsIds.Count();
            var actualProductQuantity = service.GetReceiptDetailsAsync(receiptModelId).Result.First(x => x.ProductId.Equals(productId)).Amount;
            //Assert
            Assert.AreEqual(expectedReceiptDetailsCount, actualReceiptDetailsCount);
            Assert.AreEqual(expectedProductQuantity, actualProductQuantity);
        }

        [TestMethod]
        public async Task ReceiptService_AddProductAsync_ReceiptNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var receiptModelId = UnitTestHelper.GetWrongId();
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = 1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddProductAsync(receiptModelId, productId, quantity), "Receipt not found!");
        }

        [TestMethod]
        public async Task ReceiptService_AddProductAsync_ProductNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var receiptModelId = service.GetAllAsync().Result.First().Id;
            var productId = UnitTestHelper.GetWrongId();
            decimal quantity = 1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddProductAsync(receiptModelId, productId, quantity), "Product not found!");
        }

        [TestMethod]
        public async Task ReceiptService_AddProductAsync_QuantityLessZeroException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var receiptModelId = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = -1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddProductAsync(receiptModelId, productId, quantity), "Amount should be more than 0!");
        }


        [TestMethod]
        public async Task ReceiptService_RemoveProductAsync_RemoveReceiptDetail()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 1;
            //Act
            var receiptModelId = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = 100;
            await service.RemoveProductAsync(receiptModelId, productId, quantity);
            var actual = service.GetByIdAsync(receiptModelId).Result.ReceiptDetailsIds.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ReceiptService_RemoveProductAsync_RemoveProductQuantity()
        {
            //Arrange
            var service = await CreateService();
            const int expectedReceiptDetailsCount = 2;
            //Act
            var receiptModelId = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = 1;
            var expectedProductQuantity = service.GetReceiptDetailsAsync(receiptModelId).Result.First(x => x.ProductId.Equals(productId)).Amount - quantity;
            await service.RemoveProductAsync(receiptModelId, productId, quantity);
            var actualReceiptDetailsCount = service.GetByIdAsync(receiptModelId).Result.ReceiptDetailsIds.Count();
            var actualProductQuantity = service.GetReceiptDetailsAsync(receiptModelId).Result.First(x => x.ProductId.Equals(productId)).Amount;
            //Assert
            Assert.AreEqual(expectedReceiptDetailsCount, actualReceiptDetailsCount);
            Assert.AreEqual(expectedProductQuantity, actualProductQuantity);
        }

        [TestMethod]
        public async Task ReceiptService_RemoveProductAsync_ReceiptNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var receiptModelId = UnitTestHelper.GetWrongId();
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = 1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.RemoveProductAsync(receiptModelId, productId, quantity), "Receipt not found!");
        }

        [TestMethod]
        public async Task ReceiptService_RemoveProductAsync_ProductNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var receiptModelId = service.GetAllAsync().Result.First().Id;
            var productId = UnitTestHelper.GetWrongId();
            decimal quantity = 1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.RemoveProductAsync(receiptModelId, productId, quantity), "Product not found!");
        }

        [TestMethod]
        public async Task ReceiptService_RemoveProductAsync_QuantityLessZeroException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var receiptModelId = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = -1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.RemoveProductAsync(receiptModelId, productId, quantity), "Amount should be more than 0!");
        }

    }
}
