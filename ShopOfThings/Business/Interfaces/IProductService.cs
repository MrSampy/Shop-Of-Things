using Business.Models;

namespace Business.Interfaces
{
    public interface IProductService:ICrud<ProductModel>
    {
        Task<IEnumerable<ProductModel>> GetByFilterAsync(ProductFilterSearchModel filterSearch);
        Task<IEnumerable<StorageTypeModel>> GetAllStorageTypesAsync();
        Task AddStorageTypeAsync(StorageTypeModel storageTypeModel);
        Task UpdatStorageTypeAsync(StorageTypeModel storageTypeModel);
        Task DeleteStorageTypeAsync(Guid storageTypeId);
        Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync();
        Task AddProductCategoryAsync(ProductCategoryModel productCategoryModel);
        Task UpdatProductCategoryAsync(ProductCategoryModel productCategoryModel);
        Task DeleteProductCategoryAsync(Guid productCategoryId);

    }
}
