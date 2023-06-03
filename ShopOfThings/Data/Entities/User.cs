namespace Data.Entities
{
    public class User: BaseEntity
    {
        public string NickName { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string SecondName { set; get; }
        public DateTime BirthDate { set; get; }
        public Guid UserRoleId { set; get; }
        public UserRole UserRole { set; get; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Receipt> Receipts { get; set; }

    }
}
