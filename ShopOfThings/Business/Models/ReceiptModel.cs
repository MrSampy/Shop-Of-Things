using Business.Models;
namespace Business.Models
{
    public class ReceiptModel
    {
        public int Id { set; get; }

        public int UserId { get; set; }
        
        public string ReceiptName { get; set; }

        public string ReceiptDescription { set; get; }

        public ICollection<ReceiptDetailModel>? ReceiptDetails { get; set; }

    }
}
