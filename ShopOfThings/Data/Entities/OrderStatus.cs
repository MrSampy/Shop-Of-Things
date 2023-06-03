namespace Data.Entities
{
    public class OrderStatus: BaseEntity
    {
        public string OrderStatusName { set; get; }
        public ICollection<Order> Orders { get; set; }

    }
}
