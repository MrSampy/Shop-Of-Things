using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductCategoryRepository : IRepository<ProductCategory>
    {
        private readonly ShopOfThingsDBContext context;

        public ProductCategoryRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(ProductCategory entity)
        {
            context.ProductCategories.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(ProductCategory entity)
        {
            context.ProductCategories.Remove(entity);
            context.SaveChanges();
        }

        public Task DeleteByIdAsync(Guid id)
        {
            context.ProductCategories.Remove(context.ProductCategories.First(x => x.Id.Equals(id)));
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await context.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetByIdAsync(Guid id)
        {
            return await context.ProductCategories.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(ProductCategory entity)
        {
            var updateEntity = context.ProductCategories.First(x => x.Id.Equals(entity.Id));
            updateEntity.ProductCategoryName = entity.ProductCategoryName;
            context.SaveChanges();
        }
    }
}
