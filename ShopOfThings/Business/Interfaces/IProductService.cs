using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductService:ICrud<ProductModel>
    {
        Task<IEnumerable<ProductModel>> GetByFilterAsync(ProductFilterSearchModel filterSearch);
        Task<IEnumerable<StorageTypeModel>> GetAllStorageTypesAsync();
        Task AddStorageTypeAsync(StorageTypeModel storageTypeModel);
        Task UpdatStorageTypeAsync(StorageTypeModel storageTypeModel);
        Task RemoveStorageTypeAsync(Guid storageTypeId);


    }
}
