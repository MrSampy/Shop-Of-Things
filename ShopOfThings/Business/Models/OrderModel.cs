using Data.Entities;

namespace Business.Models
{
    public class OrderModel
    {
        public Guid Id { set; get; }
        public DateTime OperationDate { get; set; }
        public Guid OrderStatusId { set; get; }
        public Guid UserId { get; set; }
        public string OrderStatusName { set; get; }
        public ICollection<Guid>? OrderDetailsIds { get; set; }
    }
}
