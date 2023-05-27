using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IOrderService:ICrud<OrderModel>
    {
        Task AddProductAsync(Guid orderId, Guid productId, decimal quantity);
        Task RemoveProductAsync(Guid orderId, Guid productId, decimal quantity);
        Task RemoveProductByIdAsync(Guid productId, Guid orderId);
        Task ChangeOrderStatus(Guid orderId, Guid orderStatusid);

        Task<IEnumerable<OrderDetailModel>> GetOrderDetailsAsync(Guid ordertId);

        Task<IEnumerable<OrderModel>> GetOrdersByPeriodAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<OrderStatusModel>> GetAlldOrderStatusesAsync();

        Task AddOrderStatusAsync(OrderStatusModel orderStatusModel);

        Task UpdatOrderStatusAsync(OrderStatusModel orderStatusModel);

        Task DeleteOrderStatusAsync(Guid orderStatusId);
    }
}
