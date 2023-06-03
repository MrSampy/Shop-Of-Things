using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
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
            if (model == null || model.OperationDate == DateTime.MinValue) 
            {
                throw new ShopOfThingsException("Wrong data for order!");
            
            }
            _ = await UnitOfWork.UserRepository.GetByIdAsync((Guid)model.UserId) ?? throw new ShopOfThingsException("User not found!");
            if (model.OrderStatusId == Guid.Empty)
            {
                var fisrtStatus = UnitOfWork.OrderStatusRepository.GetAllAsync().Result.First();
                model.OrderStatusName = fisrtStatus.OrderStatusName;
                model.OrderStatusId = fisrtStatus.Id;
            }
            else 
            {
                _ = await UnitOfWork.OrderStatusRepository.GetByIdAsync((Guid)model.OrderStatusId) ?? throw new ShopOfThingsException("Order status not found!");
            }
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

        public async Task AddProductAsync(Guid orderId, Guid productId, decimal quantity)
        {
            var order = await UnitOfWork.OrderRepository.GetByIdAsync(orderId) ?? throw new ShopOfThingsException("Order not found!");
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(productId) ?? throw new ShopOfThingsException("Product not found!");
            if (quantity <= 0)
            {
                throw new ShopOfThingsException("Quantity should be more than 0!");
            }
            if (order.OrderDetails is null || !order.OrderDetails.Any(x => x.ProductId.Equals(productId)))
            {
                var orderDetailModel = new OrderDetailModel 
                {
                    ProductId = product.Id,
                    OrderId = order.Id,
                    Quantity = quantity,
                };

                await UnitOfWork.OrderDetailRepository.AddAsync(Mapper.Map<OrderDetail>(orderDetailModel));
            }
            else 
            {
                var orderDetailModel = UnitOfWork.OrderDetailRepository.GetAllAsync().Result.First(x=>x.ProductId.Equals(product.Id));
                orderDetailModel.Quantity += quantity;
                UnitOfWork.OrderDetailRepository.Update(orderDetailModel);
            }            
        }

        public async Task DeleteAsync(Guid modelId)
        {
            var order = await UnitOfWork.OrderRepository.GetByIdAsync(modelId) ?? throw new ShopOfThingsException("Order not found!");
            UnitOfWork.OrderRepository.Delete(Mapper.Map<Order>(order));
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
            return result == null ? throw new ShopOfThingsException("Order not found!") : Mapper.Map<OrderModel>(result);
        }

        public async Task<IEnumerable<OrderDetailModel>> GetOrderDetailsAsync(Guid ordertId)
        {
            var result = (await UnitOfWork.OrderRepository.GetByIdAsync(ordertId));
            return result == null
                ? throw new ShopOfThingsException("Order not found!")
                : Mapper.Map<IEnumerable<OrderDetailModel>>(result.OrderDetails);
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var result = (await UnitOfWork.OrderRepository.GetAllAsync()).Where(x=>x.OperationDate>=startDate && x.OperationDate<=endDate).ToList();
            return Mapper.Map<IEnumerable<OrderModel>>(result);
        }

        public async Task DeleteOrderStatusAsync(Guid orderStatusId)
        {
            var result = await UnitOfWork.OrderStatusRepository.GetByIdAsync(orderStatusId) ?? throw new ShopOfThingsException("Order status not found!");
            UnitOfWork.OrderStatusRepository.Delete(result);
        }

        public async Task RemoveProductAsync(Guid orderId, Guid productId, decimal quantity)
        {
            var order = await UnitOfWork.OrderRepository.GetByIdAsync(orderId) ?? throw new ShopOfThingsException("Order not found!");
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(productId) ?? throw new ShopOfThingsException("Product not found!");
            if (quantity <= 0)
            {
                throw new ShopOfThingsException("Quantity should be more than 0!");
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
            _ = await UnitOfWork.OrderRepository.GetByIdAsync(model.Id) ?? throw new ShopOfThingsException("Order not found!");
            if (model.UserId == Guid.Empty || model.OrderStatusId == Guid.Empty || model.OperationDate == DateTime.MinValue) 
            {
                throw new ShopOfThingsException("Wrong data for order!");
            }

            _ = await UnitOfWork.OrderStatusRepository.GetByIdAsync((Guid)model.OrderStatusId) ?? throw new ShopOfThingsException("Order status not found!");
            _ = await UnitOfWork.UserRepository.GetByIdAsync((Guid)model.UserId) ?? throw new ShopOfThingsException("User not found!");
            UnitOfWork.OrderRepository.Update(Mapper.Map<Order>(model));
        }

        public async Task UpdatOrderStatusAsync(OrderStatusModel orderStatusModel)
        {
            _ = await UnitOfWork.OrderStatusRepository.GetByIdAsync(orderStatusModel.Id) ?? throw new ShopOfThingsException("Order status not found!");
            if (string.IsNullOrEmpty(orderStatusModel.OrderStatusName)) 
            {
                throw new ShopOfThingsException("Wrong data for order status!");
            }
            UnitOfWork.OrderStatusRepository.Update(Mapper.Map<OrderStatus>(orderStatusModel));
        }

        public async Task ChangeOrderStatus(Guid orderId, Guid orderStatusId)
        {
            var order = await UnitOfWork.OrderRepository.GetByIdAsync(orderId) ?? throw new ShopOfThingsException("Order not found!");
            var orderStatus = await UnitOfWork.OrderStatusRepository.GetByIdAsync(orderStatusId) ?? throw new ShopOfThingsException("Order status not found!");
            order.OrderStatusId = orderStatus.Id;
            UnitOfWork.OrderRepository.Update(order);
        }

        public async Task RemoveProductByIdAsync(Guid productId, Guid orderId)
        {
            var order = await UnitOfWork.OrderRepository.GetByIdAsync(orderId) ?? throw new ShopOfThingsException("Order not found!");
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(productId) ?? throw new ShopOfThingsException("Product not found!");
            if (order.OrderDetails != null && order.OrderDetails.Any(x => x.ProductId.Equals(productId)))
            {
                var orderDetail = order.OrderDetails.FirstOrDefault(x => x.ProductId.Equals(productId));
                if (orderDetail != null) 
                {
                    UnitOfWork.OrderDetailRepository.Delete(orderDetail);
                }
            }
        }

        public async Task<OrderPriceModel> GetOrderFullPrice(Guid orderId)
        {
            var order = await UnitOfWork.OrderRepository.GetByIdAsync(orderId) ?? throw new ShopOfThingsException("Order not found!");
            var orderDetailsUnitPrices = new List<OrderDetailPriceModel>();
            foreach (OrderDetail orderDetail in order.OrderDetails) 
            {
                orderDetailsUnitPrices.Add(new OrderDetailPriceModel
                {
                    OrderDetailId = orderDetail.Id,
                    UnitPrice = orderDetail.Quantity * orderDetail.Product.Price
                }
                );
            }
            var result = new OrderPriceModel 
            {
                FullPrice = orderDetailsUnitPrices.Select(x=>x.UnitPrice).Sum(),
                OrderDetailPrices = orderDetailsUnitPrices
            };
            return result;
        }
    }
}
