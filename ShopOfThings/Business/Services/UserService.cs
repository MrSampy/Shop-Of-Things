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
            if (string.IsNullOrEmpty(model.Email)
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
            var userRole = UnitOfWork.UserRoleRepository.GetAllAsync().Result.FirstOrDefault(x=>x.UserRoleName.Equals("Customer"));
            if (userRole == null) 
            {
                throw new ShopOfThingsException("User role not found!");
            }
            model.UserRoleName = userRole.UserRoleName;
            model.UserRoleId = userRole.Id;
            model.Password = SecurePasswordHasher.Hash(model.Password);
            await UnitOfWork.UserRepository.AddAsync(Mapper.Map<User>(model));

        }

        public async Task AddUserRoleAsync(UserRoleModel userRoleModel)
        {
            if (string.IsNullOrEmpty(userRoleModel.UserRoleName)) 
            {
                throw new ShopOfThingsException("Wrong data for user role!");
            }
            var isAlreadyExixsts = UnitOfWork.UserRoleRepository.GetAllAsync().Result.Any(x=>x.UserRoleName.Equals(userRoleModel.UserRoleName));
            if (isAlreadyExixsts)
            {
                throw new ShopOfThingsException("User role with such name already exists!");
            }
            await UnitOfWork.UserRoleRepository.AddAsync(Mapper.Map<UserRole>(userRoleModel));
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

        public async Task<IEnumerable<UserRoleModel>> GetAllUserRolesAsync()
        {
            var result = await UnitOfWork.UserRoleRepository.GetAllAsync();
            return Mapper.Map<IEnumerable<UserRoleModel>>(result);
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

        public async Task DeleteUserRoleAsync(Guid usserRoleId)
        {
            var userRole = await UnitOfWork.UserRoleRepository.GetByIdAsync(usserRoleId);
            if (userRole == null)
            {
                throw new ShopOfThingsException("User role not found!");
            }
            await UnitOfWork.UserRoleRepository.DeleteByIdAsync(usserRoleId);
        }

        public async Task UpdateAsync(UserModel model)
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(model.Id);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            if (model.UserRoleId == null || string.IsNullOrEmpty(model.Email)
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
            var userRole = await UnitOfWork.UserRoleRepository.GetByIdAsync((Guid)model.UserRoleId);
            if (userRole == null)
            {
                throw new ShopOfThingsException("User role not found!");
            }
            if (!userRole.UserRoleName.Equals(model.UserRoleName))
            {
                throw new ShopOfThingsException("User role name can`t be empty!");
            }           
            if (!model.Password.Equals(user.Password)) 
            {
                model.Password = SecurePasswordHasher.Hash(model.Password);
            }
            UnitOfWork.UserRepository.Update(Mapper.Map<User>(model));
        }

        public async Task UpdatUserRoleAsync(UserRoleModel userRoleModel)
        {
            var userRole = await UnitOfWork.UserRoleRepository.GetByIdAsync(userRoleModel.Id);
            if (userRole == null)
            {
                throw new ShopOfThingsException("User role not found!");
            }
            if (string.IsNullOrEmpty(userRoleModel.UserRoleName))
            {
                throw new ShopOfThingsException("Wrong data for user role!");
            }
            var isAlreadyExixsts = UnitOfWork.UserRoleRepository.GetAllAsync().Result.Any(x => 
            !x.Id.Equals(userRoleModel.Id) && x.UserRoleName.Equals(userRoleModel.UserRoleName));
            if (isAlreadyExixsts)
            {
                throw new ShopOfThingsException("User role with such name already exists!");
            }
            UnitOfWork.UserRoleRepository.Update(Mapper.Map<UserRole>(userRoleModel));
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

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByUserId(Guid userId)
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            return Mapper.Map<IEnumerable<ReceiptModel>>(UnitOfWork.ReceiptRepository.GetAllAsync().Result.Where(x => x.UserId.Equals(userId)));
        }

        public async Task<IEnumerable<ProductModel>> GetProductsByUserId(Guid userId)
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            return Mapper.Map<IEnumerable<ProductModel>>(UnitOfWork.ProductRepository.GetAllAsync().Result.Where(x => x.UserId.Equals(userId)));
        }
        public async Task<IEnumerable<OrderModel>> GetOrdersByUserId(Guid userId)
        {
            var user = await UnitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ShopOfThingsException("User not found!");
            }
            return Mapper.Map<IEnumerable<OrderModel>>(UnitOfWork.OrderRepository.GetAllAsync().Result.Where(x => x.UserId.Equals(userId)));
        }

    }
}
