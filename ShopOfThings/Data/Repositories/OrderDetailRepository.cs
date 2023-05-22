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
    public class OrderDetailRepository : IRepository<OrderDetail>
    {
        private readonly ShopOfThingsDBContext context;

        public OrderDetailRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(OrderDetail entity)
        {
            context.OrderDetails.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(OrderDetail entity)
        {
            context.OrderDetails.Remove(entity);
            context.SaveChanges();
        }

        public Task DeleteByIdAsync(Guid id)
        {
            context.OrderDetails.Remove(context.OrderDetails.First(x => x.Id.Equals(id)));
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await context.OrderDetails.ToListAsync();
        }

        public async Task<OrderDetail> GetByIdAsync(Guid id)
        {
            return await context.OrderDetails.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(OrderDetail entity)
        {
            var updateEntity = context.OrderDetails.First(x => x.Id.Equals(entity.Id));
            updateEntity.Quantity = entity.Quantity;
            updateEntity.ProductId = entity.ProductId;
            updateEntity.Product = entity.Product;
            updateEntity.Order = entity.Order;
            updateEntity.OrderId = entity.OrderId;
            updateEntity.UnitPrice = updateEntity.UnitPrice;
            context.SaveChanges();
        }
    }
}
