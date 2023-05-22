using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Guid? UserStatusId { set; get; }
        public UserStatus? UserStatus { set; get; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Product>? Products { get; set; }
        public ICollection<Receipt>? Receipts { get; set; }

    }
}
