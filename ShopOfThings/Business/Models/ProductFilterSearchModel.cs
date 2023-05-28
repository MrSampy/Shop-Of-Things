namespace Business.Models
{
    public class ProductFilterSearchModel
    {
        public Guid? StorageTypeId { set; get; }
        public decimal? MinPrice { set; get; }
        public decimal? MaxPrice { set; get; }
        public Guid? ProductCategoryId { get; set; }

    }
}
