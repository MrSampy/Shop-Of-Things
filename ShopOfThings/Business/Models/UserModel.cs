using Data.Entities;

namespace Business.Models
{
    public class UserModel
    {
        public Guid Id { set; get; }
        public string NickName { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string SecondName { set; get; }
        public DateTime BirthDate { set; get; }
        public Guid? UserStatusId { set; get; }
        public string? UserStatusName { set; get; }
        public virtual ICollection<Guid>? OrdersIds { get; set; }
        public ICollection<Guid>? ProductsIds { get; set; }
        public ICollection<Guid>? ReceiptsIds { get; set; }
    }
}
