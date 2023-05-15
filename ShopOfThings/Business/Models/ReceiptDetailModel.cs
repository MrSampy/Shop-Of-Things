namespace Business.Models
{
    public class ReceiptDetailModel
    {
        public int Id { set; get; }
        public int ReceiptId { set; get; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }

    }
}
