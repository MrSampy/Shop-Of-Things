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
    public class ReceiptRepository : IRepository<Receipt>
    {
        private readonly ShopOfThingsDBContext context;

        public ReceiptRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(Receipt entity)
        {
            context.Receipts.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(Receipt entity)
        {
            context.Receipts.Remove(entity);
            context.SaveChanges();
        }

        public Task DeleteByIdAsync(Guid id)
        {
            context.Receipts.Remove(context.Receipts.First(x => x.Id.Equals(id)));
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await context.Receipts.ToListAsync();
        }

        public async Task<Receipt> GetByIdAsync(Guid id)
        {
            return await context.Receipts.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(Receipt entity)
        {
            var updateEntity = context.Receipts.First(x => x.Id.Equals(entity.Id));
            updateEntity.User = entity.User;
            updateEntity.UserId = entity.UserId;
            updateEntity.ReceiptName = entity.ReceiptName;
            updateEntity.ReceiptDescription = entity.ReceiptDescription;
            context.SaveChanges();
        }
    }
}
