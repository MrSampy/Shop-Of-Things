using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IOrderService:ICrud<Order>
    {
        Task AddProductAsync(int productId, int ordertId, int quantity);

        Task RemoveProductAsync(int productId, int ordertId, int quantity);
        Task<IEnumerable<OrderDetailModel>> GetOrderDetailsAsync(int ordertId);

        Task ChangeOrderStatus(int ordertId, int orderStatusId);

        Task<IEnumerable<ReceiptModel>> GetOrdersByPeriodAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<OrderStatusModel>> GetAlldOrderStatusesAsync();

        Task AddOrderStatusAsync(OrderStatusModel orderStatusModel);

        Task UpdatOrderStatusAsync(OrderStatusModel orderStatusModel);

        Task RemoveOrderStatusAsync(int orderStatusId);
    }
}
