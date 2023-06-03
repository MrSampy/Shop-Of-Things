
namespace Data.Entities
{
    public class OrderDetail: BaseEntity
    {
        public Guid ProductId { set; get; }

        public Product Product { set; get; }
        public Guid OrderId { set; get; }

        public Order Order { set; get; }

        public decimal Quantity { set; get; }

    }
}
