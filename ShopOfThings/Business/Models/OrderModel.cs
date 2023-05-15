namespace Business.Models
{
    public class OrderModel
    {
        public int Id { set; get; }
        public DateTime OperationDate { get; set; }
        public int OrderStatusId { set; get; }
        public int UserId { get; set; }
        public ICollection<OrderDetailModel>? OrderDetails { get; set; }
    }
}
