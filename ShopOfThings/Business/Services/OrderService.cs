using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Validation;
namespace Business.Services
{
    public class OrderService : IOrderService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }

        public async Task AddAsync(OrderModel model)
        {
            if (model == null || model.OperationDate == DateTime.MinValue || model.UserId == null) 
            {
                throw new ShopOfThingsException("Wrong data for order!");
            
            }
            var fisrtStatus = UnitOfWork.OrderStatusRepository.GetAllAsync().Result.First();
            model.OrderStatusName = fisrtStatus.OrderStatusName;
            model.OrderStatusId = fisrtStatus.Id;
            await UnitOfWork.OrderRepository.AddAsync(Mapper.Map<Order>(model));
        }

        public async Task AddOrderStatusAsync(OrderStatusModel orderStatusModel)
        {
            if (orderStatusModel == null || string.IsNullOrEmpty(orderStatusModel.OrderStatusName))
            {
                throw new ShopOfThingsException("Wrong data for order status!");
            }
            await UnitOfWork.OrderStatusRepository.AddAsync(Mapper.Map<OrderStatus>(orderStatusModel));
        }

        public async Task AddProductAsync(Guid productId, Guid orderId, decimal quantity)
        {
            var order = await UnitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null) 
            {
                throw new ShopOfThingsException("Order not found!");
            }
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new ShopOfThingsException("Product not found!");
            }
            if (quantity <= 0)
            {
                throw new ShopOfThingsException("Quantity should be less than 0!");
            }
            if (order.OrderDetails is null || !order.OrderDetails.Any(x => x.ProductId.Equals(productId)))
            {
                var orderDetailModel = new OrderDetailModel 
                {
                    ProductId = product.Id,
                    OrderId = order.Id,
                    Quantity = quantity,
                    UnitPrice = quantity * product.Price
                };

                await UnitOfWork.OrderDetailRepository.AddAsync(Mapper.Map<OrderDetail>(orderDetailModel));
            }
            else 
            {
                var orderDetailModel = UnitOfWork.OrderDetailRepository.GetAllAsync().Result.First(x=>x.ProductId.Equals(product.Id));
                orderDetailModel.Quantity += quantity;
                UnitOfWork.OrderDetailRepository.Update(Mapper.Map<OrderDetail>(orderDetailModel));
            }
            
        }

        public async Task DeleteAsync(Guid modelId)
        {
            var order = await UnitOfWork.OrderRepository.GetByIdAsync(modelId);
            if (order == null)
            {
                throw new ShopOfThingsException("Order not found!");
            }
            UnitOfWork.OrderRepository.Update(Mapper.Map<Order>(order));
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            var result = await UnitOfWork.OrderRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<OrderModel>>(result);
        }

        public async Task<IEnumerable<OrderStatusModel>> GetAlldOrderStatusesAsync()
        {
            var result = await UnitOfWork.OrderStatusRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<OrderStatusModel>>(result);
        }

        public async Task<OrderModel> GetByIdAsync(Guid id)
        {
            var result = await UnitOfWork.OrderRepository.GetByIdAsync(id);
            if (result == null) 
            {
                throw new ShopOfThingsException("Order not found!");
            }
            return Mapper.Map<OrderModel>(result);
        }

        public async Task<IEnumerable<OrderDetailModel>> GetOrderDetailsAsync(Guid ordertId)
        {
            var result = (await UnitOfWork.OrderRepository.GetByIdAsync(ordertId));
            if (result == null)
            {
                throw new ShopOfThingsException("Order not found!");
            }
            return Mapper.Map<IEnumerable<OrderDetailModel>>(result.OrderDetails);
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var result = (await UnitOfWork.OrderRepository.GetAllAsync()).All(x=>x.OperationDate>=startDate && x.OperationDate<=endDate);
            return Mapper.Map<IEnumerable<OrderModel>>(result);
        }

        public async Task RemoveOrderStatusAsync(Guid orderStatusId)
        {
            var result = await UnitOfWork.OrderStatusRepository.GetByIdAsync(orderStatusId);
            if (result == null)
            {
                throw new ShopOfThingsException("Order status not found!");
            }
            UnitOfWork.OrderStatusRepository.Delete(result);
        }

        public async Task RemoveProductAsync(Guid productId, Guid orderId, decimal quantity)
        {
            var order = await UnitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new ShopOfThingsException("Order not found!");
            }
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new ShopOfThingsException("Product not found!");
            }
            if (quantity <= 0)
            {
                throw new ShopOfThingsException("Quantity should be less than 0!");
            }
            if (order.OrderDetails != null && order.OrderDetails.Any(x => x.ProductId.Equals(productId))) 
            {
                var orderDetail = order.OrderDetails.First(x=>x.ProductId.Equals(productId));
                decimal difference = orderDetail.Quantity - quantity;
                if (difference <= 0)
                {
                    UnitOfWork.OrderDetailRepository.Delete(orderDetail);
                }
                else 
                {
                    orderDetail.Quantity -= quantity;
                    UnitOfWork.OrderDetailRepository.Update(orderDetail);
                }
            }


        }

        public async Task UpdateAsync(OrderModel model)
        {
            var order = await UnitOfWork.OrderRepository.GetByIdAsync(model.Id);
            if (order == null)
            {
                throw new ShopOfThingsException("Order not found!");
            }
            if (model.UserId == null || model.OrderStatusId == null || model.OperationDate == DateTime.MinValue) 
            {
                throw new ShopOfThingsException("Wrong data for order!");
            }
            var orderStatus = await UnitOfWork.OrderStatusRepository.GetByIdAsync((Guid)model.OrderStatusId);
            if (orderStatus == null) 
            {
                throw new ShopOfThingsException("Order status not found!");
            }
            var user = await UnitOfWork.UserRepository.GetByIdAsync((Guid)model.UserId);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            order.OperationDate = model.OperationDate;
            order.OrderStatusId = model.OrderStatusId;
            order.UserId = model.UserId;
            UnitOfWork.OrderRepository.Update(order);
        }

        public async Task UpdatOrderStatusAsync(OrderStatusModel orderStatusModel)
        {
            var orderStatus = await UnitOfWork.OrderStatusRepository.GetByIdAsync(orderStatusModel.Id);
            if (orderStatus == null)
            {
                throw new ShopOfThingsException("Order status not found!");
            }
            if (string.IsNullOrEmpty(orderStatusModel.OrderStatusName)) 
            {
                throw new ShopOfThingsException("Wrong data for order status!");
            }
            orderStatus.OrderStatusName = orderStatusModel.OrderStatusName;
            UnitOfWork.OrderStatusRepository.Update(orderStatus);
        }

        public async Task ChangeStatusOrder(Guid orderId, Guid orderStatusId)
        {
            var order = await UnitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new ShopOfThingsException("Order not found!");
            }
            var orderStatus = await UnitOfWork.OrderStatusRepository.GetByIdAsync(orderStatusId);
            if (orderStatus == null)
            {
                throw new ShopOfThingsException("Order status not found!");
            }
            order.OrderStatusId = orderStatus.Id;
            UnitOfWork.OrderRepository.Update(order);
        }
    }
}
