using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IReceiptDetailRepository:IRepository<ReceiptDetail>
    {
        Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync();

        Task<ReceiptDetail> GetByIdWithDetailsAsync(int id);
    }
}
