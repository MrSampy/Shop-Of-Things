namespace Business.Models
{
    public class OrderStatusModel
    {
        public int Id { set; get; }

        public string OrderStatusName { set; get; }

        public ICollection<OrderModel>? Orders { get; set; }
    }
}
