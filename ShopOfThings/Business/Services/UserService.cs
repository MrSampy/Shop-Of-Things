using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Validation;
using Data.Entities;
using System.Text.RegularExpressions;

namespace Business.Services
{
    public class UserService : IUserService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }
        public async Task AddAsync(UserModel model)
        {
            if (model.UserStatusId == null || string.IsNullOrEmpty(model.Email)
                || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.SecondName)
                || string.IsNullOrEmpty(model.NickName) || string.IsNullOrEmpty(model.Password)) 
            {
                throw new ShopOfThingsException("Wrong data for user!");
            }
            var age = DateTime.Today.Year - model.BirthDate.Year;
            if (age <= 0 || age >= 150) 
            {
                throw new ShopOfThingsException("Wrong birth date for user!");
            }
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(model.Email);
            if (!match.Success)
            {
                throw new ShopOfThingsException("Incorrect email address!");
            }
            var isAlreadyExixsts = UnitOfWork.UserRepository.GetAllAsync().Result.Any(x=>x.NickName.Equals(model.NickName) || x.Email.Equals(model.Email));
            if (isAlreadyExixsts) 
            {
                throw new ShopOfThingsException("User with such nickname or email already exists!");
            }
            var userStatus = await UnitOfWork.UserStatusRepository.GetByIdAsync((Guid)model.UserStatusId);
            if (userStatus == null)
            {
                throw new ShopOfThingsException("User status not found!");
            }
            if (!userStatus.UserStatusName.Equals(model.UserStatusName)) 
            {
                throw new ShopOfThingsException("User status name not found!");
            }                       
            model.Password = SecurePasswordHasher.Hash(model.Password);
            await UnitOfWork.UserRepository.AddAsync(Mapper.Map<User>(model));

        }

        public async Task AddUserStatusAsync(UserStatusModel userStatusModel)
        {
            if (string.IsNullOrEmpty(userStatusModel.UserStatusName)) 
            {
                throw new ShopOfThingsException("Wrong data for user status!");
            }
            var isAlreadyExixsts = UnitOfWork.UserStatusRepository.GetAllAsync().Result.Any(x=>x.UserStatusName.Equals(userStatusModel.UserStatusName));
            if (isAlreadyExixsts)
            {
                throw new ShopOfThingsException("User status with such name already exists!");
            }
            await UnitOfWork.UserStatusRepository.AddAsync(Mapper.Map<UserStatus>(userStatusModel));
        }

        public async Task DeleteAsync(Guid modelId)
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(modelId);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            await UnitOfWork.UserRepository.DeleteByIdAsync(modelId);
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            var result = await UnitOfWork.UserRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<UserModel>>(result);
        }

        public async Task<IEnumerable<UserStatusModel>> GetAllUserStatusesAsync()
        {
            var result = await UnitOfWork.UserStatusRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<UserStatusModel>>(result);
        }

        public async Task<UserModel> GetByIdAsync(Guid id)
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            return Mapper.Map<UserModel>(user);
        }

        public async Task DeleteUserStatusAsync(Guid userStatusId)
        {
            var userStatus = await UnitOfWork.UserStatusRepository.GetByIdAsync(userStatusId);
            if (userStatus == null)
            {
                throw new ShopOfThingsException("User status not found!");
            }
            await UnitOfWork.UserStatusRepository.DeleteByIdAsync(userStatusId);
        }

        public async Task UpdateAsync(UserModel model)
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(model.Id);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            if (model.UserStatusId == null || string.IsNullOrEmpty(model.Email)
                || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.SecondName)
                || string.IsNullOrEmpty(model.NickName) || string.IsNullOrEmpty(model.Password))
            {
                throw new ShopOfThingsException("Wrong data for user!");
            }
            var age = DateTime.Today.Year - model.BirthDate.Year;
            if (age <= 0 || age >= 150)
            {
                throw new ShopOfThingsException("Wrong birth date for user!");
            }
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(model.Email);
            if (!match.Success)
            {
                throw new ShopOfThingsException("Incorrect email address!");
            }
            var isAlreadyExixsts = UnitOfWork.UserRepository.GetAllAsync().Result.Any( x=>
               !x.Id.Equals(model.Id) && (x.NickName.Equals(model.NickName) || x.Email.Equals(model.Email)));
            if (isAlreadyExixsts)
            {
                throw new ShopOfThingsException("User with such nickname or email already exists!");
            }
            var userStatus = await UnitOfWork.UserStatusRepository.GetByIdAsync((Guid)model.UserStatusId);
            if (userStatus == null)
            {
                throw new ShopOfThingsException("User status not found!");
            }
            if (!userStatus.UserStatusName.Equals(model.UserStatusName))
            {
                throw new ShopOfThingsException("User status name can`t be empty!");
            }           
            if (!model.Password.Equals(user.Password)) 
            {
                model.Password = SecurePasswordHasher.Hash(model.Password);
            }
            UnitOfWork.UserRepository.Update(Mapper.Map<User>(model));
        }

        public async Task UpdatUserStatusAsync(UserStatusModel userStatusModel)
        {
            var userStatus = await UnitOfWork.UserStatusRepository.GetByIdAsync(userStatusModel.Id);
            if (userStatus == null)
            {
                throw new ShopOfThingsException("User status not found!");
            }
            if (string.IsNullOrEmpty(userStatusModel.UserStatusName))
            {
                throw new ShopOfThingsException("Wrong data for user status!");
            }
            var isAlreadyExixsts = UnitOfWork.UserStatusRepository.GetAllAsync().Result.Any(x => 
            !x.Id.Equals(userStatusModel.Id) && x.UserStatusName.Equals(userStatusModel.UserStatusName));
            if (isAlreadyExixsts)
            {
                throw new ShopOfThingsException("User status with such name already exists!");
            }
            UnitOfWork.UserStatusRepository.Update(Mapper.Map<UserStatus>(userStatusModel));
        }

        public async Task<bool> VerifyPassword(Guid userId, string password)
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            return SecurePasswordHasher.Verify(password,user.Password);
        }

        public async Task<bool> LogIn(string nickName, string password) 
        {
            var user = UnitOfWork.UserRepository.GetAllAsync().Result.FirstOrDefault(x=>x.NickName.Equals(nickName));
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            return await VerifyPassword(user.Id,password);
        }

    }
}
