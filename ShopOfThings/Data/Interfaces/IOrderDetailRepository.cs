using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IOrderDetailRepository:IRepository<OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetAllWithDetailsAsync();

        Task<OrderDetail> GetByIdWithDetailsAsync(int id);
    }
}
