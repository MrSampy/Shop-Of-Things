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
    public class ReceiptDetailRepository : IReceiptDetailRepository
    {
        private readonly ShopOfThingsDBContext context;

        public ReceiptDetailRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(ReceiptDetail entity)
        {
            context.ReceiptDetails.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(ReceiptDetail entity)
        {
            context.ReceiptDetails.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            context.ReceiptDetails.Remove(context.ReceiptDetails.First(x => x.Id.Equals(id)));
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            return await context.ReceiptDetails.ToListAsync();
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await context.ReceiptDetails
                 .Include(x => x.Product)
                 .Include(x => x.Receipt)
                 .Include(x => x.StorageType).ToListAsync();
        }

        public async Task<ReceiptDetail> GetByIdAsync(int id)
        {
            return await context.ReceiptDetails.FirstAsync(x => x.Id.Equals(id));
        }

        public async Task<ReceiptDetail> GetByIdWithDetailsAsync(int id)
        {
            return await context.ReceiptDetails
                .Include(x => x.Product)
                .Include(x => x.Receipt)
                .Include(x => x.StorageType).FirstAsync(x => x.Id.Equals(id));
        }

        public void Update(ReceiptDetail entity)
        {
            context.ReceiptDetails.Update(entity);
        }
    }
}
