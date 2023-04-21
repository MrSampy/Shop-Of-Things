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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ShopOfThingsDBContext context;

        public OrderDetailRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(OrderDetail entity)
        {
            context.OrderDetails.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(OrderDetail entity)
        {
            context.OrderDetails.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            context.OrderDetails.Remove(context.OrderDetails.First(x => x.Id.Equals(id)));
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await context.OrderDetails.ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetAllWithDetailsAsync()
        {
            return await context.OrderDetails
                 .Include(x => x.Product)
                 .Include(x => x.Order).ToListAsync();
        }

        public async Task<OrderDetail> GetByIdAsync(int id)
        {
            return await context.OrderDetails.FirstAsync(x => x.Id.Equals(id));
        }

        public async Task<OrderDetail> GetByIdWithDetailsAsync(int id)
        {
            return await context.OrderDetails
                 .Include(x => x.Product)
                 .Include(x => x.Order).FirstAsync(x => x.Id.Equals(id));
        }

        public void Update(OrderDetail entity)
        {
            context.OrderDetails.Update(entity);
        }
    }
}
