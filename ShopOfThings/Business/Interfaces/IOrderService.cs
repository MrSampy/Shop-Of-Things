﻿using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IOrderService:ICrud<OrderModel>
    {
        Task AddProductAsync(Guid productId, Guid ordertId, decimal quantity);

        Task RemoveProductAsync(Guid productId, Guid ordertId, decimal quantity);
        Task<IEnumerable<OrderDetailModel>> GetOrderDetailsAsync(Guid ordertId);

        Task<IEnumerable<ReceiptModel>> GetOrdersByPeriodAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<OrderStatusModel>> GetAlldOrderStatusesAsync();

        Task AddOrderStatusAsync(OrderStatusModel orderStatusModel);

        Task UpdatOrderStatusAsync(OrderStatusModel orderStatusModel);

        Task RemoveOrderStatusAsync(Guid orderStatusId);
    }
}
