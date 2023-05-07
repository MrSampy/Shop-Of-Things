﻿using Data.Data;
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
    public class OrderRepository : IRepository<Order>
    {
        private readonly ShopOfThingsDBContext context;

        public OrderRepository(ShopOfThingsDBContext context)
        {
            this.context = context;
        }
        public Task AddAsync(Order entity)
        {
            context.Orders.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(Order entity)
        {
            context.Orders.Remove(entity);
            context.SaveChanges();
        }

        public Task DeleteByIdAsync(int id)
        {
            context.Orders.Remove(context.Orders.First(x => x.Id.Equals(id)));
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await context.Orders.ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await context.Orders.FirstAsync(x => x.Id.Equals(id));
        }

        public void Update(Order entity)
        {
            var updateEntity = context.Orders.First(x => x.Id.Equals(entity.Id));
            updateEntity.OrderStatus = entity.OrderStatus;
            updateEntity.OrderStatusId = entity.OrderStatusId;
            updateEntity.UserId = entity.UserId;
            updateEntity.User = entity.User;
            updateEntity.OperationDate = entity.OperationDate;
            context.SaveChanges();
        }
    }
}
