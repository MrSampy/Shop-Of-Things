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
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOfThingsDBContext context;

        public ProductRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }

        public Task AddAsync(Product entity)
        {
            context.Products.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(Product entity)
        {
            context.Products.Remove(entity);
            context.SaveChanges();
        }

        public Task DeleteByIdAsync(int id)
        {
            context.Products.Remove(context.Products.First(x => x.Id.Equals(id)));
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await context.Products
                 .Include(x => x.User)
                 .Include(x => x.StorageType).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await context.Products.FirstAsync(x => x.Id.Equals(id));
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return await context.Products
                .Include(x => x.User)
                 .Include(x => x.StorageType).FirstAsync(x => x.Id.Equals(id));
        }

        public void Update(Product entity)
        {
            var updateEntity = context.Products.First(x => x.Id.Equals(entity.Id));
            updateEntity.Price = entity.Price;
            updateEntity.ProductName = entity.ProductName;
            updateEntity.ProductDescription = entity.ProductDescription;
            updateEntity.Amount = entity.Amount;
            updateEntity.StorageType = entity.StorageType;
            updateEntity.StorageTypeId = entity.StorageTypeId;
            updateEntity.User = entity.User;
            updateEntity.UserId = entity.UserId;
            context.SaveChanges();
        }
    }
}
