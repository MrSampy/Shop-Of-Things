using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly ShopOfThingsDBContext context;

        public OrderStatusRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(OrderStatus entity)
        {
            context.OrderStatuses.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(OrderStatus entity)
        {
            context.OrderStatuses.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            context.OrderStatuses.Remove(context.OrderStatuses.First(x => x.Id.Equals(id)));
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<OrderStatus>> GetAllAsync()
        {
            return await context.OrderStatuses.ToListAsync();
        }

        public async Task<OrderStatus> GetByIdAsync(int id)
        {
            return await context.OrderStatuses.FirstAsync(x => x.Id.Equals(id));
        }

        public void Update(OrderStatus entity)
        {
            context.OrderStatuses.Update(entity);
        }
    }
}
