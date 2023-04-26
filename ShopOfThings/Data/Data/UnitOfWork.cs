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


        public IOrderDetailRepository OrderDetailRepository => orderDetailRepository ??= new OrderDetailRepository(d);

        public IOrderRepository OrderRepository => orderRepository;

        public IOrderStatusRepository OrderStatusRepository => orderStatusRepository;

        public IProductRepository ProductRepository => productRepository;

        public IReceiptRepository ReceiptRepository => receiptRepository;

        public IReceiptDetailRepository ReceiptDetailRepository => receiptDetailRepository;

        public IStorageTypeRepository StorageTypeRepository => storageTypeRepository;

        public IUserRepository UserRepository => userRepository;

        public IUserStatusRepository UserStatusRepository => userStatusRepository;

        public UnitOfWork(ShopOfThingsDBContext shopOfThingsDBContext)
        {
            this.dbContext = shopOfThingsDBContext;
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
