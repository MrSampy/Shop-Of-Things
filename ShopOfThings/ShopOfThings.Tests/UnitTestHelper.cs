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
                new User { Id = 1, Name = "Test1", SecondName = "TestN", Email = "123@test.com", Password = "123", BirthDate = DateTime.Today.AddYears(-20), UserStatusId = 1 },
                new User { Id = 2, Name = "Test2", SecondName = "TestN2", Email = "1423@test.com", Password = "1423", BirthDate = DateTime.Today.AddYears(-22), UserStatusId = 2 },
                new User { Id = 3, Name = "Test3", SecondName = "TestN4", Email = "14263@test.com", Password = "14523", BirthDate = DateTime.Today.AddYears(-25), UserStatusId = 2 }
                );
            context.Products.AddRange(
                new Product { Id = 1, ProductName = "Produict1", ProductDescription = "ProductDescription1", Price = 12.5M, Amount = 4, StorageTypeId = 1, UserId = 3 },
                new Product { Id = 2, ProductName = "Produict2", ProductDescription = "ProductDescription2", Price = 10.9M, Amount = 0, StorageTypeId = 2, UserId = 3 },
                new Product { Id = 3, ProductName = "Produict3", ProductDescription = "ProductDescription3", Price = 16, Amount = 1, StorageTypeId = 1, UserId = 2 }
                );
            context.Orders.AddRange(
                new Order { Id = 1, OperationDate = DateTime.Today.AddDays(-5), UserId = 3, OrderStatusId = 2 },
                new Order { Id = 2, OperationDate = DateTime.Today.AddDays(-6), UserId = 2, OrderStatusId = 4 }
                );
            context.OrderDetails.AddRange(
                new OrderDetail { Id = 1, OrderId = 1, ProductId = 1, Quantity = 3.4M },
                new OrderDetail { Id = 2, OrderId = 1, ProductId = 3, Quantity = 6 },
                new OrderDetail { Id = 3, OrderId = 2, ProductId = 2, Quantity = 10 }
                );
            context.Receipts.AddRange(
                new Receipt { Id = 1, ReceiptName = "R1", ReceiptDescription = "D1", UserId = 2 },
                new Receipt { Id = 2, ReceiptName = "R2", ReceiptDescription = "D2", UserId = 3 }
                );
            context.ReceiptDetails.AddRange(                
                new ReceiptDetail { Id = 1, Amount = 2, ReceiptId = 1, ProductId = 1, },
                new ReceiptDetail { Id = 2, Amount = 4.3M, ReceiptId = 1, ProductId = 3 },
                new ReceiptDetail { Id = 3, Amount = 10.8M, ReceiptId = 2, ProductId = 2 }
                );
            context.SaveChanges();

        }

    }
}
