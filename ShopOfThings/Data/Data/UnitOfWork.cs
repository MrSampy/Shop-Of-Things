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

        private UserStatusRepository userStatusRepository;


        public IOrderDetailRepository OrderDetailRepository => orderDetailRepository ??= new OrderDetailRepository(dbContext);

        public IOrderRepository OrderRepository => orderRepository ??= new OrderRepository(dbContext);

        public IOrderStatusRepository OrderStatusRepository => orderStatusRepository ??= new OrderStatusRepository(dbContext);

        public IProductRepository ProductRepository => productRepository ??= new ProductRepository(dbContext);

        public IReceiptRepository ReceiptRepository => receiptRepository ??= new ReceiptRepository(dbContext);

        public IReceiptDetailRepository ReceiptDetailRepository => receiptDetailRepository ??= new ReceiptDetailRepository(dbContext);

        public IStorageTypeRepository StorageTypeRepository => storageTypeRepository ??= new StorageTypeRepository(dbContext);

        public IUserRepository UserRepository => userRepository ??= new UserRepository(dbContext);

        public IUserStatusRepository UserStatusRepository => userStatusRepository ??= new UserStatusRepository(dbContext);

        public UnitOfWork(ShopOfThingsDBContext shopOfThingsDBContext)
        {
            this.dbContext = shopOfThingsDBContext;
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
