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
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly ShopOfThingsDBContext context;

        public ReceiptRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(Receipt entity)
        {
            context.Receipts.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(Receipt entity)
        {
            context.Receipts.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            context.Receipts.Remove(context.Receipts.First(x => x.Id.Equals(id)));
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await context.Receipts.ToListAsync();
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await context.Receipts
                 .Include(x => x.User).ToListAsync();
        }

        public async Task<Receipt> GetByIdAsync(int id)
        {
            return await context.Receipts.FirstAsync(x => x.Id.Equals(id));
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return await context.Receipts
                 .Include(x => x.User).FirstAsync(x => x.Id.Equals(id));
        }

        public void Update(Receipt entity)
        {
            context.Receipts.Update(entity);
        }
    }
}
