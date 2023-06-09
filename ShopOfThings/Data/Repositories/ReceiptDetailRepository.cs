﻿using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ReceiptDetailRepository : IRepository<ReceiptDetail>
    {
        private readonly ShopOfThingsDBContext context;

        public ReceiptDetailRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(ReceiptDetail entity)
        {
            context.ReceiptDetails.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(ReceiptDetail entity)
        {
            context.ReceiptDetails.Remove(entity);
            context.SaveChanges();
        }

        public Task DeleteByIdAsync(Guid id)
        {
            context.ReceiptDetails.Remove(context.ReceiptDetails.First(x => x.Id.Equals(id)));
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            return await context.ReceiptDetails.ToListAsync();
        }

        public async Task<ReceiptDetail> GetByIdAsync(Guid id)
        {
            return await context.ReceiptDetails.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(ReceiptDetail entity)
        {
            var updateEntity = context.ReceiptDetails.First(x => x.Id.Equals(entity.Id));
            updateEntity.Amount = entity.Amount;
            updateEntity.ProductId = entity.ProductId;
            updateEntity.ReceiptId = entity.ReceiptId;
            context.SaveChanges();
        }
    }
}
