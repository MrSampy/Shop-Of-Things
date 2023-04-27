using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data.Data;
using Data.Repositories;
using Data.Entities;

namespace ShopOfThings.Tests
{
    public class UnitTestHelper
    {

        public static DbContextOptions<ShopOfThingsDBContext> GetUnitTestDbOptions() 
        {
            var options = new DbContextOptionsBuilder<ShopOfThingsDBContext>()
                .UseInMemoryDatabase(databaseName: "database_name").Options;


            return options;
        }

        public static void SeedData(ShopOfThingsDBContext context) 
        {
            context.UserStatuses.AddRange(
                new UserStatus { Id = 1, UserStatusName = "Admin" },
                new UserStatus { Id = 2, UserStatusName = "Customer" });
            context.OrderStatuses.AddRange(
                new OrderStatus { Id = 1, OrderStatusName = "New" },
                new OrderStatus { Id = 2, OrderStatusName = "Payment_received" },
                new OrderStatus { Id = 3, OrderStatusName = "Sent" },
                new OrderStatus { Id = 4, OrderStatusName = "Received" },
                new OrderStatus { Id = 5, OrderStatusName = "Completed" },
                new OrderStatus { Id = 6, OrderStatusName = "Canceled_by_user" },
                new OrderStatus { Id = 7, OrderStatusName = "Canceled_by_the_administrator" }
                );
            context.StorageTypes.AddRange(
                new StorageType { Id = 1, StorageTypeName = "kg"},
                new StorageType { Id = 2, StorageTypeName = "thing" }
                );
            context.Users.AddRange(
                new User { Id = 1, Name = "Test1", SecondName = "TestN", Email = "123@test.com", Password = "123", BirthDate = DateTime.Today.AddYears(-20), UserStatus = context.UserStatuses.First() },
                new User { Id = 2, Name = "Test2", SecondName = "TestN2", Email = "1423@test.com", Password = "1423", BirthDate = DateTime.Today.AddYears(-22), UserStatus = context.UserStatuses.Last() },
                new User { Id = 3, Name = "Test3", SecondName = "TestN4", Email = "14263@test.com", Password = "14523", BirthDate = DateTime.Today.AddYears(-25), UserStatus = context.UserStatuses.Last() }
                );
            context.Products.AddRange(
                new Product { Id = 1, ProductName = "Produict1", ProductDescription = "ProductDescription1", Price = 12.5M, Amount = 4, StorageType = context.StorageTypes.First(), ProductOwner = context.Users.Last() },
                new Product { Id = 2, ProductName = "Produict2", ProductDescription = "ProductDescription2", Price = 10.9M, Amount = 0, StorageType = context.StorageTypes.Last(), ProductOwner = context.Users.Last() },
                new Product { Id = 3, ProductName = "Produict3", ProductDescription = "ProductDescription3", Price = 16, Amount = 1, StorageType = context.StorageTypes.First(), ProductOwner = context.Users.First(x=>x.Id.Equals(2)) }
                );
            context.Orders.AddRange(
                new Order { Id = 1, OperationDate = DateTime.Today.AddDays(-5), MasterOfOrder = context.Users.Last(), OrderStatus = context.OrderStatuses.First(x => x.Id.Equals(2)) },
                new Order { Id = 2, OperationDate = DateTime.Today.AddDays(-6), MasterOfOrder = context.Users.First(x => x.Id.Equals(2)), OrderStatus = context.OrderStatuses.First(x => x.Id.Equals(4)) }
                );
            context.OrderDetails.AddRange(
                new OrderDetail { Id = 1, Order = context.Orders.First(), Product = context.Products.First(), Quantity = 3.4M },
                new OrderDetail { Id = 2, Order = context.Orders.First(), Product = context.Products.Last(), Quantity = 6 },
                new OrderDetail { Id = 3, Order = context.Orders.Last(), Product = context.Products.First(x=>x.Id.Equals(2)), Quantity = 10 }
                );
            context.Receipts.AddRange(
                new Receipt { Id = 1, ReceiptName = "R1", ReceiptDescription = "D1", User = context.Users.First(x => x.Id.Equals(2)) },
                new Receipt { Id = 2, ReceiptName = "R2", ReceiptDescription = "D2", User = context.Users.Last() }
                );
            context.ReceiptDetails.AddRange(                
                new ReceiptDetail { Id = 1, Amount = 2, Receipt = context.Receipts.First(), Product = context.Products.First(), StorageType = context.StorageTypes.Last() },
                new ReceiptDetail { Id = 2, Amount = 4.3M, Receipt = context.Receipts.First(), Product = context.Products.Last(), StorageType = context.StorageTypes.First() },
                new ReceiptDetail { Id = 3, Amount = 10.8M, Receipt = context.Receipts.Last(), Product = context.Products.First(x=>x.Equals(2)), StorageType = context.StorageTypes.First() }
                );
            
        }

    }
}
