using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using Data.Repositories;

namespace Data.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<OrderDetail> OrderDetailRepository { get; }
        IRepository<Order> OrderRepository { get; }
        IRepository<OrderStatus> OrderStatusRepository { get; }
        IRepository<Product> ProductRepository { get; }
        IRepository<Receipt> ReceiptRepository { get; }
        IRepository<ReceiptDetail> ReceiptDetailRepository { get; }
        IRepository<StorageType> StorageTypeRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<UserStatus> UserStatusRepository { get; }

        public Task SaveAsync();
    }
}
