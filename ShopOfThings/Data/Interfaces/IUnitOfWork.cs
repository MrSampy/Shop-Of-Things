using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork
    {
        IOrderDetailsRepository OrderDetailsRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderStatusRepository OrderStatusRepository { get; }
        IProductRepository ProductRepository { get; }
        IReceiptRepository ReceiptRepository { get; }
        IReceiptDetailRepository ReceiptDetailRepository { get; }
        IStorageTypeRepository StorageTypeRepository { get; }
        IUserRepository UserRepository { get; }

        public Task SaveAsync();
    }
}
