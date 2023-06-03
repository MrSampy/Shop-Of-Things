namespace Data.Entities
{
    public class UserRole:BaseEntity
    {
        public string UserRoleName { set; get; }

        public ICollection<User> Users { get; set; }
    }
}
