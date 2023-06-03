using Business.Interfaces;
using Business.Models;
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
                || model.StorageTypeId == Guid.Empty || model.UserId == Guid.Empty || model.ProductCategoryId == Guid.Empty)
            {
                throw new ShopOfThingsException("Wrong data for product!");
            }
            _ = await UnitOfWork.UserRepository.GetByIdAsync((Guid)model.UserId) ?? throw new ShopOfThingsException("User not found!");
            _ = await UnitOfWork.StorageTypeRepository.GetByIdAsync((Guid)model.StorageTypeId) ?? throw new ShopOfThingsException("Storage type not found!");
            _ = await UnitOfWork.ProductCategoryRepository.GetByIdAsync((Guid)model.ProductCategoryId) ?? throw new ShopOfThingsException("Product category not found!");
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
            _ = await UnitOfWork.ProductRepository.GetByIdAsync(modelId) ?? throw new ShopOfThingsException("Product not found!");
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
            if (filterSearch.StorageTypeId != Guid.Empty) 
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
            return product == null ? throw new ShopOfThingsException("Product not found!") : Mapper.Map<ProductModel>(product);
        }

        public async Task DeleteStorageTypeAsync(Guid storageTypeId)
        {
            _ = await UnitOfWork.StorageTypeRepository.GetByIdAsync(storageTypeId) ?? throw new ShopOfThingsException("Storage type not found!");
            await UnitOfWork.StorageTypeRepository.DeleteByIdAsync(storageTypeId);
        }
        public async Task DeleteProductCategoryAsync(Guid productCategoryId)
        {
            _ = await UnitOfWork.ProductCategoryRepository.GetByIdAsync(productCategoryId) ?? throw new ShopOfThingsException("Product category not found!");
            await UnitOfWork.ProductCategoryRepository.DeleteByIdAsync(productCategoryId);
        }

        public async Task UpdateAsync(ProductModel model)
        {
            _ = await UnitOfWork.ProductRepository.GetByIdAsync(model.Id) ?? throw new ShopOfThingsException("Product not found!");
            if (model == null || model.Amount == null || model.Price == null || model.Amount < 0 
                || string.IsNullOrEmpty(model.ProductName) || string.IsNullOrEmpty(model.ProductDescription) 
                || model.Price <= 0 || model.StorageTypeId == Guid.Empty || model.UserId == Guid.Empty || model.ProductCategoryId == Guid.Empty)
            {
                throw new ShopOfThingsException("Wrong data for product!");
            }
            _ = await UnitOfWork.UserRepository.GetByIdAsync((Guid)model.UserId) ?? throw new ShopOfThingsException("User not found!");
            _ = await UnitOfWork.StorageTypeRepository.GetByIdAsync((Guid)model.StorageTypeId) ?? throw new ShopOfThingsException("Storage Type not found!");
            _ = await UnitOfWork.ProductCategoryRepository.GetByIdAsync((Guid)model.ProductCategoryId) ?? throw new ShopOfThingsException("Product category not found!");
            UnitOfWork.ProductRepository.Update(Mapper.Map<Product>(model));
        }

        public async Task UpdatStorageTypeAsync(StorageTypeModel storageTypeModel)
        {
            _ = await UnitOfWork.StorageTypeRepository.GetByIdAsync(storageTypeModel.Id) ?? throw new ShopOfThingsException("Storage type not found!");
            if (string.IsNullOrEmpty(storageTypeModel.StorageTypeName))
            {
                throw new ShopOfThingsException("Wrong data for storage type!");
            }
            UnitOfWork.StorageTypeRepository.Update(Mapper.Map<StorageType>(storageTypeModel));

        }


        public async Task UpdatProductCategoryAsync(ProductCategoryModel productCategoryModel)
        {
            var productCategory = await UnitOfWork.ProductCategoryRepository.GetByIdAsync(productCategoryModel.Id) ?? throw new ShopOfThingsException("Product category type not found!");
            if (string.IsNullOrEmpty(productCategory.ProductCategoryName))
            {
                throw new ShopOfThingsException("Wrong data for product category!");
            }
            UnitOfWork.ProductCategoryRepository.Update(Mapper.Map<ProductCategory>(productCategory));
        }

    }
}
