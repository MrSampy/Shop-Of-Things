using Data.Entities;

namespace Business.Models
{
    public class UserStatusModel
    {
        public Guid Id { set; get; }
        public string UserStatusName { set; get; }
        public ICollection<Guid>? UsersIds { get; set; }
    }
}
