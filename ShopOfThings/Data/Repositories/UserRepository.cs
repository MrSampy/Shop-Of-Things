using Data.Entities;
using Data.Interfaces;
using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository : IRepository<User>
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

        public Task DeleteByIdAsync(Guid id)
        {
            context.Users.Remove(context.Users.First(x => x.Id.Equals(id)));
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await context.Users.FirstOrDefaultAsync(x=>x.Id.Equals(id));
        }

        public void Update(User entity)
        {
            var updateEntity = context.Users.First(x => x.Id.Equals(entity.Id));
            updateEntity.UserRoleId = entity.UserRoleId;
            updateEntity.Password = entity.Password;
            updateEntity.BirthDate = entity.BirthDate;
            updateEntity.Name = entity.Name;
            updateEntity.SecondName = entity.SecondName;
            updateEntity.Email = entity.Email;
            updateEntity.NickName = entity.NickName;
            context.SaveChanges();
        }
    }
}
