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
        Task<bool> LogIn(string nickName, string password);
        Task<bool> VerifyPassword(Guid userId, string password);
        Task<IEnumerable<UserStatusModel>> GetAllUserStatusesAsync();
        Task AddUserStatusAsync(UserStatusModel userStatusModel);
        Task UpdatUserStatusAsync(UserStatusModel userStatusModel);
        Task DeleteUserStatusAsync(Guid userStatusId);

    }
}
