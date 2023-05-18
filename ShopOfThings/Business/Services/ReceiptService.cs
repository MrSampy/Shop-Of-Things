using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Validation;
using Data.Data;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;
        public ReceiptService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }
        public async Task AddAsync(ReceiptModel model)
        {
            if (model.UserId == null || string.IsNullOrEmpty(model.ReceiptName) || string.IsNullOrEmpty(model.ReceiptDescription))
            {
                throw new ShopOfThingsException("Wrong data for receipt!");
            }
            var user = await UnitOfWork.UserRepository.GetByIdAsync((Guid)model.UserId);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            await UnitOfWork.ReceiptRepository.AddAsync(Mapper.Map<Receipt>(model));
        }

        public async Task AddProductAsync(Guid productId, Guid receiptId, decimal amount)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
            if (receipt == null)
            {
                throw new ShopOfThingsException("Receipt not found!");
            }
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new ShopOfThingsException("Product not found!");
            }
            if (amount <= 0)
            {
                throw new ShopOfThingsException("Amount should be more than 0!");
            }
            if (receipt.ReceiptDetails is null || !receipt.ReceiptDetails.Any(x => x.ProductId.Equals(productId)))
            {
                var receiptDetailModel = new ReceiptDetailModel 
                {
                    ProductId = productId,
                    Amount = amount,
                    ReceiptId = receiptId
                };
                await UnitOfWork.ReceiptDetailRepository.AddAsync(Mapper.Map<ReceiptDetail>(receiptDetailModel));
            }
            else
            {
                var receiptDetail= UnitOfWork.ReceiptDetailRepository.GetAllAsync().Result.First(x => x.ProductId.Equals(product.Id));
                receiptDetail.Amount += amount;
                UnitOfWork.ReceiptDetailRepository.Update(receiptDetail);
            }
        }

        public async Task DeleteAsync(Guid modelId)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdAsync(modelId);
            if (receipt == null)
            {
                throw new ShopOfThingsException("Receipt not found!");
            }
            UnitOfWork.ReceiptRepository.Delete(receipt);
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var result = await UnitOfWork.ReceiptRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<ReceiptModel>>(result);
        }

        public async Task<ReceiptModel> GetByIdAsync(Guid id)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdAsync(id);
            if (receipt == null)
            {
                throw new ShopOfThingsException("Receipt not found!");
            }
            return Mapper.Map<ReceiptModel>(receipt);
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(Guid receipttId)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdAsync(receipttId);
            if (receipt == null)
            {
                throw new ShopOfThingsException("Receipt not found!");
            }
            return Mapper.Map<IEnumerable<ReceiptDetailModel>>(receipt);
        }

        public async Task RemoveProductAsync(Guid productId, Guid receiptId, decimal amount)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
            if (receipt == null)
            {
                throw new ShopOfThingsException("Receipt not found!");
            }
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new ShopOfThingsException("Product not found!");
            }
            if (amount <= 0)
            {
                throw new ShopOfThingsException("Amount should be more than 0!");
            }
            if (receipt.ReceiptDetails != null && receipt.ReceiptDetails.Any(x => x.ProductId.Equals(productId)))
            {
                var receiptDetail = receipt.ReceiptDetails.First(x => x.ProductId.Equals(productId));
                decimal difference = receiptDetail.Amount - amount;
                if (difference <= 0)
                {
                    UnitOfWork.ReceiptDetailRepository.Delete(receiptDetail);
                }
                else
                {
                    receiptDetail.Amount -= amount;
                    UnitOfWork.ReceiptDetailRepository.Update(receiptDetail);
                }
            }
        }

        public async Task RemoveProductByIdAsync(Guid productId, Guid receiptId)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
            if (receipt == null)
            {
                throw new ShopOfThingsException("Receipt not found!");
            }
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new ShopOfThingsException("Product not found!");
            }
            if (receipt.ReceiptDetails != null && receipt.ReceiptDetails.Any(x => x.ProductId.Equals(productId)))
            {
                var receiptDetail = receipt.ReceiptDetails.First(x => x.ProductId.Equals(productId));
                UnitOfWork.ReceiptDetailRepository.Delete(receiptDetail);
            }
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdAsync(model.Id);
            if (receipt == null)
            {
                throw new ShopOfThingsException("Receipt not found!");
            }
            if (model.UserId == null || string.IsNullOrEmpty(model.ReceiptName) || string.IsNullOrEmpty(model.ReceiptDescription))
            {
                throw new ShopOfThingsException("Wrong data for receipt!");
            }
            var user = await UnitOfWork.UserRepository.GetByIdAsync((Guid)model.UserId);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            UnitOfWork.ReceiptRepository.Update(Mapper.Map<Receipt>(model));
        }

        public async Task UpdatReceiptDetailAsync(ReceiptDetailModel receiptDetailModel)
        {
            var receiptDetail = await UnitOfWork.ReceiptDetailRepository.GetByIdAsync(receiptDetailModel.Id);
            if (receiptDetail == null)
            {
                throw new ShopOfThingsException("Receipt detail not found!");
            }
            if(receiptDetailModel.ProductId == null || receiptDetailModel.ReceiptId == null || receiptDetailModel.Amount <= 0) 
            {
                throw new ShopOfThingsException("Wrong data for receipt detail!");
            }
            var receipt = await UnitOfWork.ReceiptRepository.GetByIdAsync((Guid)receiptDetailModel.ReceiptId);
            if (receipt == null)
            {
                throw new ShopOfThingsException("Receipt not found!");
            }
            var product = await UnitOfWork.ProductRepository.GetByIdAsync((Guid)receiptDetailModel.ProductId);
            if (product == null)
            {
                throw new ShopOfThingsException("Product not found!");
            }
            UnitOfWork.ReceiptDetailRepository.Update(Mapper.Map<ReceiptDetail>(receiptDetailModel));

        }
    }
}
