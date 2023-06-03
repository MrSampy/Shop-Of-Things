using Data.Entities;

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
        IRepository<UserRole> UserRoleRepository { get; }
        IRepository<ProductCategory> ProductCategoryRepository { get; }

    }
}
