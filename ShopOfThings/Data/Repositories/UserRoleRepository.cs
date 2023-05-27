using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRoleRepository : IRepository<UserRole>
    {
        private readonly ShopOfThingsDBContext context;

        public UserRoleRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(UserRole entity)
        {
            context.UserRoles.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(UserRole entity)
        {
            context.UserRoles.Remove(entity);
            context.SaveChanges();

        }

        public Task DeleteByIdAsync(Guid id)
        {
            context.UserRoles.Remove(context.UserRoles.First(x => x.Id.Equals(id)));
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<UserRole>> GetAllAsync()
        {
            return await context.UserRoles.ToListAsync();
        }

        public async Task<UserRole> GetByIdAsync(Guid id)
        {
            return await context.UserRoles.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(UserRole entity)
        {
            var updateEntity = context.UserRoles.First(x=>x.Id.Equals(entity.Id));
            updateEntity.UserRoleName = entity.UserRoleName;
            context.SaveChanges();            
        }
    }
}
