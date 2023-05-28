using Data.Entities;

namespace Business.Models
{
    public class UserRoleModel
    {
        public Guid Id { set; get; }
        public string UserRoleName { set; get; }
        public ICollection<Guid> UsersIds { get; set; }
    }
}
