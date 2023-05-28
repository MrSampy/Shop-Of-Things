namespace Business.Models
{
    public class ReceiptDetailModel
    {
        public Guid Id { set; get; }
        public Guid ReceiptId { set; get; }
        public Guid ProductId { get; set; }
        public decimal Amount { get; set; }

    }
}
