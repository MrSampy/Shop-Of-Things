using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopOfThingsDBContext context;

        public UserRepository(ShopOfThingsDBContext context) 
        {
            this.context = context;
        }

        public Task AddAsync(User entity)
        {
            context.Users.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(User entity)
        {
            context.Users.Remove(entity);
            context.SaveChanges();
        }

        public Task DeleteByIdAsync(int id)
        {
            context.Users.Remove(context.Users.First(x => x.Id.Equals(id)));
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllWithDetailsAsync()
        {
            return await context.Users
                .Include(x=>x.UserStatus).ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await context.Users.FirstAsync(x=>x.Id.Equals(id));
        }

        public async Task<User> GetByIdWithDetailsAsync(int id)
        {
            return await context.Users.Include(x => x.UserStatus).FirstAsync(x => x.Id.Equals(id));
        }

        public void Update(User entity)
        {
            var updateEntity = context.Users.First(x => x.Id.Equals(entity.Id));
            updateEntity.UserStatus = entity.UserStatus;
            updateEntity.UserStatusId = entity.UserStatusId;
            updateEntity.Password = entity.Password;
            updateEntity.BirthDate = entity.BirthDate;
            updateEntity.Name = entity.Name;
            updateEntity.SecondName = entity.SecondName;
            updateEntity.Email = entity.Email;
            context.SaveChanges();
        }
    }
}
