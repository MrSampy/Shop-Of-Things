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
        Task AddProductAsync(Guid productId, Guid receiptId, decimal amount);

        Task RemoveProductAsync(Guid productId, Guid receiptId, decimal amount);
        Task RemoveProductByIdAsync(Guid productId, Guid receiptId);
        Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(Guid receiptId);

        Task UpdatReceiptDetailAsync(ReceiptDetailModel receiptDetailModel);
    }
}
