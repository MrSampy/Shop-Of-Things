namespace Business.Models
{
    public class ProductModel
    {
        public Guid Id { set; get; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public Guid UserId { get; set; }
        public decimal Price { get; set; }
        public Guid StorageTypeId { get; set; }
        public string StorageTypeName { get; set; }
        public Guid ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public decimal Amount { get; set; }
        public ICollection<Guid> OrderDetailsIds { get; set; }
        public ICollection<Guid> ReceiptDetailsIds { get; set; }
    }
}
