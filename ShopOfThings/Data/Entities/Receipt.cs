namespace Data.Entities
{
    public class Receipt: BaseEntity
    {
        public Guid UserId { get; set; }

        public User User { get; set; }
        
        public string ReceiptName { get; set; }

        public string ReceiptDescription { set; get; }

        public ICollection<ReceiptDetail> ReceiptDetails { get; set; }

    }
}
