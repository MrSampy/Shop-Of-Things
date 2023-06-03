namespace Business.Models
{
    public class ProductCategoryModel
    {
        public Guid Id { get; set; }

        public string ProductCategoryName { set; get; }

        public ICollection<Guid> ProductsIds { set; get; }
    }
}
