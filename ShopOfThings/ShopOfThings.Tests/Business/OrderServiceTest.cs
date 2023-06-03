using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ShopOfThings.Tests.UnitTestHelpers;

namespace ShopOfThings.Tests.Business
{
    [TestClass]
    public class OrderServiceTest
    {
        private async Task<OrderService> CreateService()
        {
            var context = new ShopOfThingsDBContext(new DbContextOptionsBuilder<ShopOfThingsDBContext>()
               .EnableSensitiveDataLogging()
               .UseInMemoryDatabase(databaseName: "Test_Database").Options);
            await UnitTestHelper.SeedData(context);
            return new OrderService(UnitTestHelper.CreateUnitOfWork(context), UnitTestHelper.CreateMapper());
        }

        [TestMethod]
        public async Task OrderService_GetAllAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 2;
            //Act
            var orders = await service.GetAllAsync();
            var actual = orders.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task OrderService_GetAlldOrderStatusesAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 2;
            //Act
            var orders = await service.GetAllAsync();
            var actual = orders.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task OrderService_GetByIdAsync()
        {
            //Arrange
            var service = await CreateService();
            Guid expected = service.UnitOfWork.UserRepository.GetAllAsync().Result.Last().Id;
            //Act
            var orders = await service.GetAllAsync();
            var orderId = orders.First().Id;
            var order = await service.GetByIdAsync(orderId);
            var actual = order.UserId;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task OrderService_GetByIdAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.GetByIdAsync(id), "Order not found!");
        }

        [TestMethod]
        public async Task OrderService_GetOrderDetailsAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 2;
            //Act
            var orders = await service.GetAllAsync();
            var orderId = orders.First().Id;
            var orderDetails = await service.GetOrderDetailsAsync(orderId);
            var actual = orderDetails.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task OrderService_GetOrderDetailsAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.GetOrderDetailsAsync(id), "Order not found!");
        }

        [TestMethod]
        public async Task OrderService_GetOrdersByPeriodAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 1;
            //Act
            var orders = await service.GetOrdersByPeriodAsync(DateTime.Today.AddDays(-6), DateTime.Today.AddDays(1));
            var actual = orders.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task OrderService_AddAsync() 
        {
            //Arrange
            var service = await CreateService();
            const int expected = 3;
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var orderModel = new OrderModel
            {
                UserId = userModelId,
                OperationDate = DateTime.Today.AddDays(-2),
            };
            await service.AddAsync(orderModel);
            var actual = service.GetAllAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task OrderService_AddAsync_UserIdNotFoundl()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderStatusId = service.UnitOfWork.OrderStatusRepository.GetAllAsync().Result.First().Id;
            var orderModel = new OrderModel
            {
                OperationDate = DateTime.Today.AddDays(-2),
                OrderStatusId = orderStatusId
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(orderModel), "User not found!");
        }

        [TestMethod]
        public async Task OrderService_AddAsync_OrderStatusNotFoundl()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var orderModel = new OrderModel
            {
                UserId = userModelId,
                OperationDate = DateTime.Today.AddDays(-2),
                OrderStatusId = UnitTestHelper.GetWrongId()
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(orderModel), "Order status not found!");
        }

        [TestMethod]
        public async Task OrderService_AddAsync_OperationDateNull()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var orderStatusId = service.UnitOfWork.OrderStatusRepository.GetAllAsync().Result.First().Id;
            var orderModel = new OrderModel
            {
                UserId = userModelId,
                OrderStatusId = orderStatusId
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddAsync(orderModel), "Wrong data for order!");
        }

        [TestMethod]
        public async Task OrderService_UpdateAsync()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var orderStatusId = service.UnitOfWork.OrderStatusRepository.GetAllAsync().Result.First().Id;
            var orderModel = service.GetAllAsync().Result.First();
            orderModel.OperationDate = DateTime.Today.AddDays(-5);
            orderModel.UserId = userModelId;
            orderModel.OrderStatusId = orderStatusId;
            await service.UpdateAsync(orderModel);
            var actual = service.GetAllAsync().Result.First();
            var isEqual = orderModel.UserId == actual.UserId && orderModel.OrderStatusId == actual.OrderStatusId
                && orderModel.OperationDate == actual.OperationDate;
            //Assert
            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        public async Task OrderService_UpdateAsync_OrderNotFound()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var orderStatusId = service.UnitOfWork.OrderStatusRepository.GetAllAsync().Result.First().Id;
            var orderModel = service.GetAllAsync().Result.First();
            orderModel.OperationDate = DateTime.Today.AddDays(-5);
            orderModel.UserId = userModelId;
            orderModel.OrderStatusId = orderStatusId;
            orderModel.Id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(orderModel), "Order not found!");
        }

        [TestMethod]
        public async Task OrderService_UpdateAsync_UserNotFound()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderStatusId = service.UnitOfWork.OrderStatusRepository.GetAllAsync().Result.First().Id;
            var orderModel = service.GetAllAsync().Result.First();
            orderModel.OperationDate = DateTime.Today.AddDays(-5);
            orderModel.UserId = UnitTestHelper.GetWrongId();
            orderModel.OrderStatusId = orderStatusId;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(orderModel), "User not found!");
        }

        [TestMethod]
        public async Task OrderService_UpdateAsync_OrderStatusNotFound()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var orderModel = service.GetAllAsync().Result.First();
            orderModel.OperationDate = DateTime.Today.AddDays(-5);
            orderModel.UserId = userModelId;
            orderModel.OrderStatusId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(orderModel), "Order status not found!");
        }

        [TestMethod]
        public async Task OrderService_UpdateAsync_OperationDateNull()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var userModelId = service.UnitOfWork.UserRepository.GetAllAsync().Result.First().Id;
            var orderStatusId = service.UnitOfWork.OrderStatusRepository.GetAllAsync().Result.First().Id;
            var orderModel = service.GetAllAsync().Result.First();
            orderModel.OperationDate = DateTime.MinValue;
            orderModel.UserId = userModelId;
            orderModel.OrderStatusId = orderStatusId;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdateAsync(orderModel), "Wrong data for order!");
        }

        [TestMethod]
        public async Task OrderService_AddOrderStatusAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 8;
            //Act
            var orderStatusModel = new OrderStatusModel
            {
               OrderStatusName = "123"
            };
            await service.AddOrderStatusAsync(orderStatusModel);
            var actual = service.GetAlldOrderStatusesAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task OrderService_AddOrderStatusAsync_WrongData()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderStatusModel = new OrderStatusModel
            {
                OrderStatusName = ""
            };
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddOrderStatusAsync(orderStatusModel), "Wrong data for order status!");
        }

        [TestMethod]
        public async Task OrderService_UpdatOrderStatusAsync()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderStatusModel = service.GetAlldOrderStatusesAsync().Result.First();
            orderStatusModel.OrderStatusName = "123";
            await service.UpdatOrderStatusAsync(orderStatusModel);
            var actual = service.GetAlldOrderStatusesAsync().Result.First();
            var isEqual = orderStatusModel.OrderStatusName == actual.OrderStatusName;
            //Assert
            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        public async Task OrderService_UpdatOrderStatusAsync_NotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderStatusModel = service.GetAlldOrderStatusesAsync().Result.First();
            orderStatusModel.OrderStatusName = "123";
            orderStatusModel.Id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdatOrderStatusAsync(orderStatusModel), "Order status not found!");
        }

        [TestMethod]
        public async Task OrderService_UpdatOrderStatusAsync_StatusNameNull()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderStatusModel = service.GetAlldOrderStatusesAsync().Result.First();
            orderStatusModel.OrderStatusName = "";
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.UpdatOrderStatusAsync(orderStatusModel), "Wrong data for order status!");
        }

        [TestMethod]
        public async Task OrderService_GetOrderFullPrice()
        {
            //Arrange
            var service = await CreateService();
            const decimal expectedPrice = 138.50M;
            const int expectedOrderDetailsCount = 2;

            //Act
            var orderModel = service.GetAllAsync().Result.First();
            var actual = await service.GetOrderFullPrice(orderModel.Id);
            //Assert
            Assert.AreEqual(expectedPrice, actual.FullPrice);
            Assert.AreEqual(expectedOrderDetailsCount,actual.OrderDetailPrices.Count());
        }

        [TestMethod]
        public async Task OrderService_GetOrderFullPrice_OrderNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.GetOrderFullPrice(orderModelId), "Order not found!");

        }

        [TestMethod]
        public async Task OrderService_RemoveProductByIdAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expectedOrderDetailsCount = 1;
            //Act
            var orderModelId = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            await service.RemoveProductByIdAsync(productId, orderModelId);
            var actual = service.GetAllAsync().Result.First().OrderDetailsIds.Count();
            //Assert
            Assert.AreEqual(expectedOrderDetailsCount,actual);
        }

        [TestMethod]
        public async Task OrderService_RemoveProductByIdAsync_NoChanges()
        {
            //Arrange
            var service = await CreateService();
            const int expectedOrderDetailsCount = 2;
            //Act
            var orderModelId = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.Last().Id;
            await service.RemoveProductByIdAsync(productId, orderModelId);
            var actual = service.GetAllAsync().Result.First().OrderDetailsIds.Count();
            //Assert
            Assert.AreEqual(expectedOrderDetailsCount, actual);
        }

        [TestMethod]
        public async Task OrderService_RemoveProductByIdAsync_OrderNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelId = UnitTestHelper.GetWrongId();
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.Last().Id;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.RemoveProductByIdAsync(productId, orderModelId), "Order not found!");
        }

        [TestMethod]
        public async Task OrderService_RemoveProductByIdAsync_ProductNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelId = service.GetAllAsync().Result.First().Id;
            var productId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.RemoveProductByIdAsync(productId, orderModelId), "Product not found!");
        }

        [TestMethod]
        public async Task OrderService_ChangeOrderStatus()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelId = service.GetAllAsync().Result.First().Id;
            var orderStatusModel = service.GetAlldOrderStatusesAsync().Result.Last();
            var expected = orderStatusModel.OrderStatusName;
            await service.ChangeOrderStatus(orderModelId, orderStatusModel.Id);
            var actual = service.GetAllAsync().Result.First().OrderStatusName;
            //Assert
            Assert.AreEqual(expected,actual);
        }

        [TestMethod]
        public async Task OrderService_ChangeOrderStatus_OrderNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelId = UnitTestHelper.GetWrongId();
            var orderStatusModelId = service.GetAlldOrderStatusesAsync().Result.Last().Id;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.ChangeOrderStatus(orderModelId, orderStatusModelId), "Order not found!");
        }

        [TestMethod]
        public async Task OrderService_ChangeOrderStatus_OrderStatusNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelId = service.GetAllAsync().Result.First().Id;
            var orderStatusModelId = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.ChangeOrderStatus(orderModelId, orderStatusModelId), "Order status not found!");
        }

        [TestMethod]
        public async Task OrderService_DeleteAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 1;
            //Act
            var id = service.GetAllAsync().Result.First().Id;
            await service.DeleteAsync(id);
            var actual = service.GetAllAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task OrderService_DeleteAsync_OrderNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.DeleteAsync(id), "Order not found!");
        }

        [TestMethod]
        public async Task OrderService_DeleteOrderStatusAsync()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 6;
            //Act
            var id = service.GetAlldOrderStatusesAsync().Result.First().Id;
            await service.DeleteOrderStatusAsync(id);
            var actual = service.GetAlldOrderStatusesAsync().Result.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task OrderService_DeleteOrderStatusAsync_OrderStatusNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var id = UnitTestHelper.GetWrongId();
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.DeleteOrderStatusAsync(id), "Order status not found!");
        }

        [TestMethod]
        public async Task OrderService_AddProductAsync_AddOrderDetail()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 3;
            //Act
            var orderModelid = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.Last().Id;
            decimal quantity = 1;
            await service.AddProductAsync(orderModelid, productId, quantity);
            var actual = service.GetByIdAsync(orderModelid).Result.OrderDetailsIds.Count();
            //Assert
            Assert.AreEqual(expected,actual);
        }

        [TestMethod]
        public async Task OrderService_AddProductAsync_AddProductQuantity()
        {
            //Arrange
            var service = await CreateService();
            const int expectedOrderDetailsCount = 2;
            //Act
            var orderModelid = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = 1;
            var expectedProductQuantity = service.GetOrderDetailsAsync(orderModelid).Result.First(x=>x.ProductId.Equals(productId)).Quantity + quantity;
            await service.AddProductAsync(orderModelid, productId, quantity);
            var actualOrderDetailsCount = service.GetByIdAsync(orderModelid).Result.OrderDetailsIds.Count();
            var actualProductQuantity = service.GetOrderDetailsAsync(orderModelid).Result.First(x => x.ProductId.Equals(productId)).Quantity;
            //Assert
            Assert.AreEqual(expectedOrderDetailsCount, actualOrderDetailsCount);
            Assert.AreEqual(expectedProductQuantity,actualProductQuantity);
        }

        [TestMethod]
        public async Task OrderService_AddProductAsync_OrderNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelid = UnitTestHelper.GetWrongId();
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = 1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddProductAsync(orderModelid,productId, quantity), "Order not found!");
        }

        [TestMethod]
        public async Task OrderService_AddProductAsync_ProductNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelid = service.GetAllAsync().Result.First().Id;
            var productId = UnitTestHelper.GetWrongId();
            decimal quantity = 1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddProductAsync(orderModelid, productId, quantity), "Product not found!");
        }

        [TestMethod]
        public async Task OrderService_AddProductAsync_QuantityLessZeroException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelid = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = -1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.AddProductAsync(orderModelid, productId, quantity), "Quantity should be more than 0!");
        }

        [TestMethod]
        public async Task OrderService_RemoveProductAsync_RemoveOrderDetail()
        {
            //Arrange
            var service = await CreateService();
            const int expected = 1;
            //Act
            var orderModelid = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = 100;
            await service.RemoveProductAsync(orderModelid, productId, quantity);
            var actual = service.GetByIdAsync(orderModelid).Result.OrderDetailsIds.Count();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task OrderService_RemoveProductAsync_RemoveProductQuantity()
        {
            //Arrange
            var service = await CreateService();
            const int expectedOrderDetailsCount = 2;
            //Act
            var orderModelid = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = 1;
            var expectedProductQuantity = service.GetOrderDetailsAsync(orderModelid).Result.First(x => x.ProductId.Equals(productId)).Quantity - quantity;
            await service.RemoveProductAsync(orderModelid, productId, quantity);
            var actualOrderDetailsCount = service.GetByIdAsync(orderModelid).Result.OrderDetailsIds.Count();
            var actualProductQuantity = service.GetOrderDetailsAsync(orderModelid).Result.First(x => x.ProductId.Equals(productId)).Quantity;
            //Assert
            Assert.AreEqual(expectedOrderDetailsCount, actualOrderDetailsCount);
            Assert.AreEqual(expectedProductQuantity, actualProductQuantity);
        }

        [TestMethod]
        public async Task OrderService_RemoveProductAsync_OrderNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelid = UnitTestHelper.GetWrongId();
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = 1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.RemoveProductAsync(orderModelid, productId, quantity), "Order not found!");
        }

        [TestMethod]
        public async Task OrderService_RemoveProductAsync_ProductNotFoundException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelid = service.GetAllAsync().Result.First().Id;
            var productId = UnitTestHelper.GetWrongId();
            decimal quantity = 1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.RemoveProductAsync(orderModelid, productId, quantity), "Product not found!");
        }

        [TestMethod]
        public async Task OrderService_RemoveProductAsync_QuantityLessZeroException()
        {
            //Arrange
            var service = await CreateService();
            //Act
            var orderModelid = service.GetAllAsync().Result.First().Id;
            var productId = service.UnitOfWork.ProductRepository.GetAllAsync().Result.First().Id;
            decimal quantity = -1;
            //Assert
            await Assert.ThrowsExceptionAsync<ShopOfThingsException>(() => service.RemoveProductAsync(orderModelid, productId, quantity), "Quantity should be more than 0!");
        }
    }
}
