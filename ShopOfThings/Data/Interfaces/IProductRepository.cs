using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    internal interface IProductRepository:IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithDetailsAsync();

        Task<Product> GetByIdWithDetailsAsync(int id);
    }
}
