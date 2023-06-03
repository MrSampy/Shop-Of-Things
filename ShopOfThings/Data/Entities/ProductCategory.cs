namespace Data.Entities
{
    public class ProductCategory:BaseEntity
    {
        public string ProductCategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
