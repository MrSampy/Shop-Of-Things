using Business.Models;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IReceiptService:ICrud<ReceiptModel>
    {
        Task AddProductAsync(Guid productId, Guid ordertId, decimal quantity);

        Task RemoveProductAsync(Guid productId, Guid ordertId, decimal quantity);
        Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(Guid receipttId);

        Task UpdatReceiptDetailAsync(ReceiptDetailModel receiptDetailModel);
    }
}
