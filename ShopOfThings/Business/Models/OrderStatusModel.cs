namespace Business.Models
{
    public class OrderStatusModel
    {
        public Guid Id { set; get; }
        public string OrderStatusName { set; get; }
        public ICollection<Guid> OrdersIds { get; set; }
    }
}
