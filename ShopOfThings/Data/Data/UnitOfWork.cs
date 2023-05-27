using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ShopOfThingsDBContext dbContext;

        private OrderDetailRepository orderDetailRepository;

        private OrderRepository orderRepository;

        private OrderStatusRepository orderStatusRepository;

        private ProductRepository productRepository;

        private ReceiptRepository receiptRepository;

        private ReceiptDetailRepository receiptDetailRepository;

        private StorageTypeRepository storageTypeRepository;

        private UserRepository userRepository;

        private UserRoleRepository userRoleRepository;

        private ProductCategoryRepository productCategoryRepository;

        public IRepository<OrderDetail> OrderDetailRepository => orderDetailRepository ??= new OrderDetailRepository(dbContext);

        public IRepository<Order> OrderRepository => orderRepository ??= new OrderRepository(dbContext);

        public IRepository<OrderStatus> OrderStatusRepository => orderStatusRepository ??= new OrderStatusRepository(dbContext);

        public IRepository<Product> ProductRepository => productRepository ??= new ProductRepository(dbContext);

        public IRepository<Receipt> ReceiptRepository => receiptRepository ??= new ReceiptRepository(dbContext);

        public IRepository<ReceiptDetail> ReceiptDetailRepository => receiptDetailRepository ??= new ReceiptDetailRepository(dbContext);

        public IRepository<StorageType> StorageTypeRepository => storageTypeRepository ??= new StorageTypeRepository(dbContext);

        public IRepository<User> UserRepository => userRepository ??= new UserRepository(dbContext);

        public IRepository<UserRole> UserRoleRepository => userRoleRepository ??= new UserRoleRepository(dbContext);

        public IRepository<ProductCategory> ProductCategoryRepository => productCategoryRepository ??= new ProductCategoryRepository(dbContext);

        public UnitOfWork(ShopOfThingsDBContext shopOfThingsDBContext)
        {
            this.dbContext = shopOfThingsDBContext;
        }

    }
}
