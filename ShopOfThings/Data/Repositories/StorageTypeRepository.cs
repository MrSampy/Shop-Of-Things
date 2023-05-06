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
    public class StorageTypeRepository : IStorageTypeRepository
    {
        private readonly ShopOfThingsDBContext context;

        public StorageTypeRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(StorageType entity)
        {
            context.StorageTypes.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(StorageType entity)
        {
            context.StorageTypes.Remove(entity);
            context.SaveChanges();
        }

        public Task DeleteByIdAsync(int id)
        {
            context.StorageTypes.Remove(context.StorageTypes.First(x => x.Id.Equals(id)));
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<StorageType>> GetAllAsync()
        {
            return await context.StorageTypes.ToListAsync();
        }

        public async Task<StorageType> GetByIdAsync(int id)
        {
            return await context.StorageTypes.FirstAsync(x => x.Id.Equals(id));
        }

        public void Update(StorageType entity)
        {
            var updateEntity = context.StorageTypes.First(x => x.Id.Equals(entity.Id));
            updateEntity.StorageTypeName = entity.StorageTypeName;
            context.SaveChanges();
        }
    }
}
