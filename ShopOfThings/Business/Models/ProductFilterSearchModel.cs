namespace Business.Models
{
    public class ProductFilterSearchModel
    {
        public Guid? StorageTypeId { set; get; }
        public int? MinPrice { set; get; }
        public int? MaxPrice { set; get; }
        public Guid? ProductCategoryId { get; set; }

    }
}
