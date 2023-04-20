using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IOrderDetailsRepository:IRepository<OrderDetails>
    {
        Task<IEnumerable<OrderDetails>> GetAllWithDetailsAsync();

        Task<OrderDetails> GetByIdWithDetailsAsync(int id);
    }
}
