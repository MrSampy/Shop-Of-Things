using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserService:ICrud<UserModel>
    {
        Task<IEnumerable<OrderModel>> GetOrdersByUserId(Guid userId);
        Task<IEnumerable<ProductModel>> GetProductsByUserId(Guid userId);
        Task<IEnumerable<ReceiptModel>> GetReceiptsByUserId(Guid userId);
        Task<bool> LogIn(string nickName, string password);
        Task<bool> VerifyPassword(Guid userId, string password);
        Task<IEnumerable<UserRoleModel>> GetAllUserRolesAsync();
        Task AddUserRoleAsync(UserRoleModel userRoleModel);
        Task UpdatUserRoleAsync(UserRoleModel userRoleModel);
        Task DeleteUserRoleAsync(Guid userRoleId);

    }
}
