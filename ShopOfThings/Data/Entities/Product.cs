﻿namespace Data.Entities
{
    public class Product: BaseEntity
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid StorageTypeId { get; set; }
        public StorageType StorageType { get; set; }
        public Guid ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<ReceiptDetail> ReceiptDetails { get; set; }

    }
}
