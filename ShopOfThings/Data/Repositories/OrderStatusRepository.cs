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
    public class OrderStatusRepository : IRepository<OrderStatus>
    {
        private readonly ShopOfThingsDBContext context;

        public OrderStatusRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(OrderStatus entity)
        {
            context.OrderStatuses.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(OrderStatus entity)
        {
            context.OrderStatuses.Remove(entity);
            context.SaveChanges();
        }

        public Task DeleteByIdAsync(Guid id)
        {
            context.OrderStatuses.Remove(context.OrderStatuses.First(x => x.Id.Equals(id)));
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<OrderStatus>> GetAllAsync()
        {
            return await context.OrderStatuses.ToListAsync();
        }

        public async Task<OrderStatus> GetByIdAsync(Guid id)
        {
            return await context.OrderStatuses.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(OrderStatus entity)
        {
            var updateEntity = context.OrderStatuses.First(x => x.Id.Equals(entity.Id));
            updateEntity.OrderStatusName = entity.OrderStatusName;
            context.SaveChanges();
        }
    }
}
