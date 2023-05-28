using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Validation;
using AutoMapper;
using Data.Interfaces;
using Data.Entities;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }
        public async Task AddAsync(ProductModel model)
        {
            if (model == null || model.Amount==null || model.Price == null || model.Amount < 0 || string.IsNullOrEmpty(model.ProductName)
                || string.IsNullOrEmpty(model.ProductDescription) || model.Price <= 0 
                || model.StorageTypeId == null || model.UserId == null || model.ProductCategoryId == null)
            {
                throw new ShopOfThingsException("Wrong data for product!");
            }
            var user = await UnitOfWork.UserRepository.GetByIdAsync((Guid)model.UserId);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            var storageType = await UnitOfWork.StorageTypeRepository.GetByIdAsync((Guid)model.StorageTypeId);
            if (storageType == null) 
            {
                throw new ShopOfThingsException("Storage type not found!");
            }
            var productCategory = await UnitOfWork.ProductCategoryRepository.GetByIdAsync((Guid)model.ProductCategoryId);
            if (productCategory == null)
            {
                throw new ShopOfThingsException("Product category not found!");
            }
            await UnitOfWork.ProductRepository.AddAsync(Mapper.Map<Product>(model));
        }

        public async Task AddStorageTypeAsync(StorageTypeModel storageTypeModel)
        {
            if (string.IsNullOrEmpty(storageTypeModel.StorageTypeName)) 
            {
                throw new ShopOfThingsException("Wrong data for storage type!");
            }
            await UnitOfWork.StorageTypeRepository.AddAsync(Mapper.Map<StorageType>(storageTypeModel));
        }

        public async Task DeleteAsync(Guid modelId)
        {
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(modelId);
            if (product == null)
            {
                throw new ShopOfThingsException("Product not found!");
            }
            await UnitOfWork.ProductRepository.DeleteByIdAsync(modelId);
        }
        public async Task AddProductCategoryAsync(ProductCategoryModel productCategoryModel)
        {
            if (string.IsNullOrEmpty(productCategoryModel.ProductCategoryName))
            {
                throw new ShopOfThingsException("Wrong data for product category!");
            }
            await UnitOfWork.ProductCategoryRepository.AddAsync(Mapper.Map<ProductCategory>(productCategoryModel));
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var result = await UnitOfWork.ProductRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<ProductModel>>(result);
        }

        public async Task<IEnumerable<StorageTypeModel>> GetAllStorageTypesAsync()
        {
            var result = await UnitOfWork.StorageTypeRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<StorageTypeModel>>(result);
        }
        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var result = await UnitOfWork.ProductCategoryRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<ProductCategoryModel>>(result);
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(ProductFilterSearchModel filterSearch)
        {
            var products = await GetAllAsync();
            if (filterSearch.StorageTypeId != null) 
            {
                products = products.Where(product=>product.StorageTypeId.Equals(filterSearch.StorageTypeId));
            }
            if (filterSearch.ProductCategoryId != null)
            {
                products = products.Where(product => product.ProductCategoryId.Equals(filterSearch.ProductCategoryId));
            }
            if (filterSearch.MinPrice != null)
            {
                products = products.Where(product => product.Price >= filterSearch.MinPrice);
            }
            if (filterSearch.MaxPrice != null)
            {
                products = products.Where(product => product.Price <= filterSearch.MaxPrice);
            }
            return Mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<ProductModel> GetByIdAsync(Guid id)
        {
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new ShopOfThingsException("Product not found!");
            }
            return Mapper.Map<ProductModel>(product);
        }

        public async Task DeleteStorageTypeAsync(Guid storageTypeId)
        {
            var storageType = await UnitOfWork.StorageTypeRepository.GetByIdAsync(storageTypeId);
            if (storageType == null)
            {
                throw new ShopOfThingsException("Storage type not found!");
            }
            await UnitOfWork.StorageTypeRepository.DeleteByIdAsync(storageTypeId);
        }
        public async Task DeleteProductCategoryAsync(Guid productCategoryId)
        {
            var productCategory = await UnitOfWork.ProductCategoryRepository.GetByIdAsync(productCategoryId);
            if (productCategory == null)
            {
                throw new ShopOfThingsException("Product category not found!");
            }
            await UnitOfWork.ProductCategoryRepository.DeleteByIdAsync(productCategoryId);
        }

        public async Task UpdateAsync(ProductModel model)
        {
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(model.Id);
            if (product == null)
            {
                throw new ShopOfThingsException("Product not found!");
            }
            if (model == null || model.Amount == null || model.Price == null || model.Amount < 0 
                || string.IsNullOrEmpty(model.ProductName) || string.IsNullOrEmpty(model.ProductDescription) 
                || model.Price <= 0 || model.StorageTypeId == null || model.UserId == null || model.ProductCategoryId == null)
            {
                throw new ShopOfThingsException("Wrong data for product!");
            }
            var user = await UnitOfWork.UserRepository.GetByIdAsync((Guid)model.UserId);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            var storageType = await UnitOfWork.StorageTypeRepository.GetByIdAsync((Guid)model.StorageTypeId);
            if (storageType == null)
            {
                throw new ShopOfThingsException("Storage Type not found!");
            }
            var productCategory = await UnitOfWork.ProductCategoryRepository.GetByIdAsync((Guid)model.ProductCategoryId);
            if (productCategory == null)
            {
                throw new ShopOfThingsException("Product category not found!");
            }
            UnitOfWork.ProductRepository.Update(Mapper.Map<Product>(model));
        }

        public async Task UpdatStorageTypeAsync(StorageTypeModel storageTypeModel)
        {
            var storageType = await UnitOfWork.StorageTypeRepository.GetByIdAsync(storageTypeModel.Id);
            if (storageType == null)
            {
                throw new ShopOfThingsException("Storage type not found!");
            }
            if (string.IsNullOrEmpty(storageTypeModel.StorageTypeName))
            {
                throw new ShopOfThingsException("Wrong data for storage type!");
            }
            UnitOfWork.StorageTypeRepository.Update(Mapper.Map<StorageType>(storageTypeModel));

        }


        public async Task UpdatProductCategoryAsync(ProductCategoryModel productCategoryModel)
        {
            var productCategory = await UnitOfWork.ProductCategoryRepository.GetByIdAsync(productCategoryModel.Id);
            if (productCategory == null)
            {
                throw new ShopOfThingsException("Product category type not found!");
            }
            if (string.IsNullOrEmpty(productCategory.ProductCategoryName))
            {
                throw new ShopOfThingsException("Wrong data for product category!");
            }
            UnitOfWork.ProductCategoryRepository.Update(Mapper.Map<ProductCategory>(productCategory));
        }

    }
}
