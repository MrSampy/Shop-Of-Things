using Data.Entities;

namespace Business.Models
{
    public class StorageTypeModel
    {
        public Guid Id { set; get; }
        public string StorageTypeName { get; set; }
        public ICollection<Guid>? ProductsIds { get; set; }
    }
}
