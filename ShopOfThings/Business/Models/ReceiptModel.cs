using Business.Models;
namespace Business.Models
{
    public class ReceiptModel
    {
        public Guid Id { set; get; }

        public Guid UserId { get; set; }
        
        public string ReceiptName { get; set; }

        public string ReceiptDescription { set; get; }

        public ICollection<Guid>? ReceiptDetailsIds { get; set; }

    }
}
