using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data.Data;
using Data.Repositories;
using Data.Entities;
using System.Runtime.CompilerServices;
using Business.Validation;
using AutoMapper;
using Data.Interfaces;
using System.Security.Cryptography;

namespace ShopOfThings.Tests.UnitTestHelpers
{
    public class UnitTestHelper
    {
        public static Guid GetWrongId() 
        {
            byte[] bytes;
            new RNGCryptoServiceProvider().GetBytes(bytes = new byte[16]);
            return new Guid(bytes);
        }
        public static IUnitOfWork CreateUnitOfWork(ShopOfThingsDBContext context) 
        {
            var unitOfWork = new UnitOfWork(context);
            return unitOfWork;
        }
        public static IMapper CreateMapper() 
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
        public async static Task SeedData(ShopOfThingsDBContext context)
        {
            await context.UserStatuses.AddRangeAsync(
                new UserStatus { UserStatusName = "Admin" },
                new UserStatus { UserStatusName = "Customer" });
            await context.ProductCategories.AddRangeAsync(
                new ProductCategory { ProductCategoryyName = "Liquid" },
                new ProductCategory { ProductCategoryyName = "Meat" });
            await context.OrderStatuses.AddRangeAsync(
                new OrderStatus { OrderStatusName = "New" },
                new OrderStatus { OrderStatusName = "Payment_received" },
                new OrderStatus { OrderStatusName = "Sent" },
                new OrderStatus { OrderStatusName = "Received" },
                new OrderStatus { OrderStatusName = "Completed" },
                new OrderStatus { OrderStatusName = "Canceled_by_user" },
                new OrderStatus { OrderStatusName = "Canceled_by_the_administrator" }
                );
            await context.StorageTypes.AddRangeAsync(
                new StorageType { StorageTypeName = "kg" }, 
                new StorageType { StorageTypeName = "thing" }
                );

            await context.Users.AddRangeAsync(
                new User {NickName = "123",  Name = "Test1", SecondName = "TestN", Email = "123@test.com", Password = SecurePasswordHasher.Hash("123"), BirthDate = DateTime.Today.AddYears(-20) }, 
                new User { NickName = "asdff", Name = "Test2", SecondName = "TestN2", Email = "1423@test.com", Password = SecurePasswordHasher.Hash("1423"), BirthDate = DateTime.Today.AddYears(-22) }, 
                new User { NickName = "Nick", Name = "Test3", SecondName = "TestN4", Email = "14263@test.com", Password = SecurePasswordHasher.Hash("14523"), BirthDate = DateTime.Today.AddYears(-25) }
                );
            await context.Products.AddRangeAsync(
                new Product { ProductName = "Produict1", ProductDescription = "ProductDescription1", Price = 12.5M, Amount = 4 }, 
                new Product { ProductName = "Produict2", ProductDescription = "ProductDescription2", Price = 10.9M, Amount = 0 },
                new Product { ProductName = "Produict3", ProductDescription = "ProductDescription3", Price = 16, Amount = 1 }
                );
            await context.Orders.AddRangeAsync(
                new Order { OperationDate = DateTime.Today.AddDays(-5) },
                new Order { OperationDate = DateTime.Today.AddDays(-6) }
                );
            await context.OrderDetails.AddRangeAsync(
                new OrderDetail { Quantity = 3.4M },
                new OrderDetail { Quantity = 6 },
                new OrderDetail { Quantity = 10 }
                );
            await context.Receipts.AddRangeAsync(
                new Receipt { ReceiptName = "R1", ReceiptDescription = "D1" },
                new Receipt { ReceiptName = "R2", ReceiptDescription = "D2" }
                );
            await context.ReceiptDetails.AddRangeAsync(
                new ReceiptDetail { Amount = 2 },
                new ReceiptDetail { Amount = 4.3M },
                new ReceiptDetail { Amount = 10.8M }
                );

            await context.SaveChangesAsync();

            await AddDependencies(context);
        }

        public async static Task AddDependencies(ShopOfThingsDBContext context) 
        {
            var userStatuses = await context.UserStatuses.ToListAsync();
            var orderStatuses = await context.OrderStatuses.ToListAsync();
            var storageTypes = await context.StorageTypes.ToListAsync();
            var users = await context.Users.ToListAsync();
            var products = await context.Products.ToListAsync();
            var orders = await context.Orders.ToListAsync();
            var orderDetails = await context.OrderDetails.ToListAsync();
            var receipts = await context.Receipts.ToListAsync();
            var receiptDetails = await context.ReceiptDetails.ToListAsync();
            var productCategories = await context.ProductCategories.ToListAsync();

            users[0].UserStatusId = userStatuses[0].Id;
            users[1].UserStatusId = userStatuses[1].Id;
            users[2].UserStatusId = userStatuses[1].Id;

            products[0].UserId = users[2].Id;
            products[1].UserId = users[2].Id;
            products[2].UserId = users[1].Id;
            products[0].StorageTypeId = storageTypes[0].Id;
            products[1].StorageTypeId = storageTypes[1].Id;
            products[2].StorageTypeId = storageTypes[0].Id;
            products[0].ProductCategoryId = productCategories[0].Id;
            products[1].ProductCategoryId = productCategories[1].Id;
            products[2].ProductCategoryId = productCategories[0].Id;

            orders[0].UserId = users[2].Id;
            orders[1].UserId = users[1].Id;
            orders[0].OrderStatusId = orderStatuses[1].Id;
            orders[1].OrderStatusId = orderStatuses[3].Id;

            orderDetails[0].OrderId = orders[0].Id;
            orderDetails[1].OrderId = orders[0].Id;
            orderDetails[2].OrderId = orders[1].Id;
            orderDetails[0].ProductId = products[0].Id;
            orderDetails[1].ProductId = products[2].Id;
            orderDetails[2].ProductId = products[1].Id;

            receipts[0].UserId = users[1].Id;
            receipts[1].UserId = users[2].Id;

            receiptDetails[0].ReceiptId = receipts[0].Id;
            receiptDetails[1].ReceiptId = receipts[0].Id;
            receiptDetails[2].ReceiptId = receipts[1].Id;
            receiptDetails[0].ProductId = products[0].Id;
            receiptDetails[1].ProductId = products[2].Id;
            receiptDetails[2].ProductId = products[1].Id;

            await context.SaveChangesAsync();
        }

    }
}
