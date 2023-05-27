using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Data.Data
{
    public class ShopOfThingsDBContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Receipt> Receipts => Set<Receipt>();
        public DbSet<ReceiptDetail> ReceiptDetails => Set<ReceiptDetail>();
        public DbSet<StorageType> StorageTypes => Set<StorageType>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public ShopOfThingsDBContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        } 
        public ShopOfThingsDBContext(DbContextOptions<ShopOfThingsDBContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

        }

    }
}
