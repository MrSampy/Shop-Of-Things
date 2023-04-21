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
    public class OrderRepository : IOrderRepository
    {
        private readonly ShopOfThingsDBContext context;

        public OrderRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(Order entity)
        {
            context.Orders.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(Order entity)
        {
            context.Orders.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            context.Orders.Remove(context.Orders.First(x => x.Id.Equals(id)));
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        {
            return await context.Orders
                 .Include(x => x.OrderStatus).ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await context.Orders.FirstAsync(x => x.Id.Equals(id));
        }

        public async Task<Order> GetByIdWithDetailsAsync(int id)
        {
            return await context.Orders
                .Include(x => x.OrderStatus).FirstAsync(x => x.Id.Equals(id));
        }

        public void Update(Order entity)
        {
            context.Orders.Update(entity);
        }
    }
}
