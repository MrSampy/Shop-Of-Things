namespace Business.Models
{
    public class UserModel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string SecondName { set; get; }
        public DateTime BirthDate { set; get; }
        public int UserStatusId { set; get; }
        public virtual ICollection<OrderModel>? Orders { get; set; }
        public ICollection<ProductModel>? Products { get; set; }
        public ICollection<ReceiptModel>? Receipts { get; set; }
    }
}
