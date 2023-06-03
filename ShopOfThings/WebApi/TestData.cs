using Business.Validation;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi
{
    public class TestData
    {
        public async static Task SeedData(ShopOfThingsDBContext context)
        {
            await context.UserRoles.AddRangeAsync(
                new UserRole { UserRoleName = "Admin" },
                new UserRole { UserRoleName = "Customer" });
            await context.ProductCategories.AddRangeAsync(
                new ProductCategory { ProductCategoryName = "Liquid" },
                new ProductCategory { ProductCategoryName = "Meat" });
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
                new User { NickName = "123", Name = "Test1", SecondName = "TestN", Email = "123@test.com", Password = SecurePasswordHasher.Hash("123"), BirthDate = DateTime.Today.AddYears(-20) },
                new User { NickName = "asdff", Name = "Test2", SecondName = "TestN2", Email = "1423@test.com", Password = SecurePasswordHasher.Hash("1423"), BirthDate = DateTime.Today.AddYears(-22) },
                new User { NickName = "Nick", Name = "Test3", SecondName = "TestN4", Email = "14263@test.com", Password = SecurePasswordHasher.Hash("14523"), BirthDate = DateTime.Today.AddYears(-25) }
                );
            await context.Products.AddRangeAsync(
                new Product { ProductName = "Produict1", ProductDescription = "ProductDescription1", Price = 12.5M, Amount = 4 },
                new Product { ProductName = "Produict2", ProductDescription = "ProductDescription2", Price = 10.9M, Amount = 0 },
                new Product { ProductName = "Produict3", ProductDescription = "ProductDescription3", Price = 16, Amount = 1 },
                new Product { ProductName = "Produict4", ProductDescription = "ProductDescription4", Price = 18.3M, Amount = 75 },
                new Product { ProductName = "Produict5", ProductDescription = "ProductDescription5", Price = 5, Amount = 23 },
                new Product { ProductName = "Produict6", ProductDescription = "ProductDescription6", Price = 12, Amount = 34 }
                );
            await context.Orders.AddRangeAsync(
                new Order { OperationDate = DateTime.Today.AddDays(-5) },
                new Order { OperationDate = DateTime.Today.AddDays(-8) }
                );
            await context.OrderDetails.AddRangeAsync(
                new OrderDetail { Quantity = 3.4M },
                new OrderDetail { Quantity = 6 },
                new OrderDetail { Quantity = 10 }
                );
            await context.Receipts.AddRangeAsync(
                new Receipt { ReceiptName = "R1", ReceiptDescription = "D1" },
                new Receipt { ReceiptName = "R2", ReceiptDescription = "D2" },
                new Receipt { ReceiptName = "R3", ReceiptDescription = "D3" },
                new Receipt { ReceiptName = "R4", ReceiptDescription = "D4" },
                new Receipt { ReceiptName = "R5", ReceiptDescription = "D5" },
                new Receipt { ReceiptName = "R6", ReceiptDescription = "D6" },
                new Receipt { ReceiptName = "R7", ReceiptDescription = "D7" },
                new Receipt { ReceiptName = "R8", ReceiptDescription = "D8" }
                );
            await context.ReceiptDetails.AddRangeAsync(
                new ReceiptDetail { Amount = 2 },
                new ReceiptDetail { Amount = 4.3M },
                new ReceiptDetail { Amount = 10.8M },
                new ReceiptDetail { Amount = 3 },
                new ReceiptDetail { Amount = 1.56M },
                new ReceiptDetail { Amount = 7 },
                new ReceiptDetail { Amount = 6.3M },
                new ReceiptDetail { Amount = 9.7M },
                new ReceiptDetail { Amount = 5 },
                new ReceiptDetail { Amount = 16 },
                new ReceiptDetail { Amount = 21.5M },
                new ReceiptDetail { Amount = 13.8M }
                );

            await context.SaveChangesAsync();

            await AddDependencies(context);
        }

        public async static Task AddDependencies(ShopOfThingsDBContext context)
        {
            var userRoles = await context.UserRoles.ToListAsync();
            var orderStatuses = await context.OrderStatuses.ToListAsync();
            var storageTypes = await context.StorageTypes.ToListAsync();
            var users = await context.Users.ToListAsync();
            var products = await context.Products.ToListAsync();
            var orders = await context.Orders.ToListAsync();
            var orderDetails = await context.OrderDetails.ToListAsync();
            var receipts = await context.Receipts.ToListAsync();
            var receiptDetails = await context.ReceiptDetails.ToListAsync();
            var productCategories = await context.ProductCategories.ToListAsync();

            users[0].UserRoleId = userRoles[0].Id;
            users[1].UserRoleId = userRoles[1].Id;
            users[2].UserRoleId = userRoles[1].Id;

            products[0].UserId = users[2].Id;
            products[1].UserId = users[1].Id;
            products[2].UserId = users[1].Id;
            products[3].UserId = users[2].Id;
            products[4].UserId = users[2].Id;
            products[5].UserId = users[1].Id;
            products[0].StorageTypeId = storageTypes[0].Id;
            products[1].StorageTypeId = storageTypes[1].Id;
            products[2].StorageTypeId = storageTypes[0].Id;
            products[3].StorageTypeId = storageTypes[1].Id;
            products[4].StorageTypeId = storageTypes[1].Id;
            products[5].StorageTypeId = storageTypes[0].Id;
            products[0].ProductCategoryId = productCategories[1].Id;
            products[1].ProductCategoryId = productCategories[0].Id;
            products[2].ProductCategoryId = productCategories[1].Id;
            products[3].ProductCategoryId = productCategories[1].Id;
            products[4].ProductCategoryId = productCategories[0].Id;
            products[5].ProductCategoryId = productCategories[1].Id;


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
            receipts[2].UserId = users[1].Id;
            receipts[3].UserId = users[2].Id;
            receipts[4].UserId = users[0].Id;
            receipts[5].UserId = users[0].Id;
            receipts[6].UserId = users[1].Id;
            receipts[7].UserId = users[0].Id;

            receiptDetails[0].ReceiptId = receipts[0].Id;
            receiptDetails[1].ReceiptId = receipts[0].Id;
            receiptDetails[2].ReceiptId = receipts[1].Id;
            receiptDetails[3].ReceiptId = receipts[2].Id;
            receiptDetails[4].ReceiptId = receipts[2].Id;
            receiptDetails[5].ReceiptId = receipts[3].Id;
            receiptDetails[6].ReceiptId = receipts[4].Id;
            receiptDetails[7].ReceiptId = receipts[4].Id;
            receiptDetails[8].ReceiptId = receipts[5].Id;
            receiptDetails[9].ReceiptId = receipts[5].Id;
            receiptDetails[10].ReceiptId = receipts[6].Id;
            receiptDetails[11].ReceiptId = receipts[7].Id;

            receiptDetails[0].ProductId = products[0].Id;
            receiptDetails[1].ProductId = products[2].Id;
            receiptDetails[2].ProductId = products[1].Id;
            receiptDetails[3].ProductId = products[3].Id;
            receiptDetails[4].ProductId = products[3].Id;
            receiptDetails[5].ProductId = products[4].Id;
            receiptDetails[6].ProductId = products[4].Id;
            receiptDetails[7].ProductId = products[5].Id;
            receiptDetails[8].ProductId = products[5].Id;
            receiptDetails[9].ProductId = products[1].Id;
            receiptDetails[10].ProductId = products[5].Id;
            receiptDetails[11].ProductId = products[4].Id;

            await context.SaveChangesAsync();
        }
    }
}
