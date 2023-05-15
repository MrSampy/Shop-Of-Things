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
        Task<IEnumerable<UserStatusModel>> GetAllUserStatusesAsync();
        Task AddUserStatusAsync(UserStatusModel userStatusModel);
        Task UpdatUserStatusAsync(UserStatusModel UuserStatusModel);
        Task RemoveUserStatusAsync(int userStatus);
    }
}
