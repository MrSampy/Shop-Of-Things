namespace Data.Entities
{
    public class Order: BaseEntity
    {
        public DateTime OperationDate { get; set; }
        public Guid OrderStatusId { set; get; }
        public OrderStatus OrderStatus { set; get; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
