namespace Business.Models
{
    public class OrderDetailModel
    {
        public Guid Id { set; get; }

        public Guid ProductId { set; get; }

        public Guid OrderId { set; get; }

        public decimal Quantity { set; get; }

    }
}
