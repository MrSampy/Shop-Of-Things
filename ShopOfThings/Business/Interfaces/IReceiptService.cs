using Business.Models;

namespace Business.Interfaces
{
    public interface IReceiptService:ICrud<ReceiptModel>
    {
        Task AddProductAsync(Guid receiptId, Guid productId, decimal amount);
        Task RemoveProductAsync(Guid receiptId, Guid productId, decimal amount);
        Task RemoveProductByIdAsync(Guid productId, Guid receiptId);
        Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(Guid receiptId);

        Task UpdatReceiptDetailAsync(ReceiptDetailModel receiptDetailModel);
    }
}
