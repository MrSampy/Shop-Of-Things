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
    public class UserStatusRepository : IUserStatusRepository
    {
        private readonly ShopOfThingsDBContext context;

        public UserStatusRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(UserStatus entity)
        {
            context.UserStatuses.Add(entity);
            return Task.CompletedTask;
        }

        public void Delete(UserStatus entity)
        {
            context.UserStatuses.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            context.UserStatuses.Remove(context.UserStatuses.First(x => x.Id.Equals(id)));
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<UserStatus>> GetAllAsync()
        {
            return await context.UserStatuses.ToListAsync();
        }

        public async Task<UserStatus> GetByIdAsync(int id)
        {
            return await context.UserStatuses.FirstAsync(x => x.Id.Equals(id));
        }

        public void Update(UserStatus entity)
        {
            context.UserStatuses.Update(entity);
        }
    }
}
