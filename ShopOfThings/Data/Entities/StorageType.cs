namespace Data.Entities
{
    public class StorageType: BaseEntity
    {
        public string StorageTypeName { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
