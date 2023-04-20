using Data.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IOrderRepository:IRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();

        Task<Order> GetByIdWithDetailsAsync(int id);
    }
}
