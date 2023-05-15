namespace Business.Models
{
    public class StorageTypeModel
    {
        public int Id { set; get; }
        public string StorageTypeName { get; set; }
        public ICollection<ProductModel>? Products { get; set; }

    }
}
