using Data.Entities;

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
        public string UserStatusName { set; get; }
        public virtual ICollection<int>? OrdersIds { get; set; }
        public ICollection<int>? ProductsIds { get; set; }
        public ICollection<int>? ReceiptsIds { get; set; }
    }
}
