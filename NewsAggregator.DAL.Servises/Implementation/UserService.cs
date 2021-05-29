using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Servises.Interfaces;


namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string GetPasswordHash(string modelPassword)
        {
            const string specialValue = "123123123132";
            var sha256 = new SHA256CryptoServiceProvider();
            var sha256data = sha256.ComputeHash(Encoding.UTF8.GetBytes(modelPassword));
            var hashedPassword = Encoding.UTF8.GetString(sha256data);
            return hashedPassword;
        }

        public async Task<bool> RegisterUser(UserDto model)
        {
            try
            {
                await _unitOfWork.User.Add(new User()
                {
                    Id = model.Id,
                    Email = model.Email,
                    Login = "Mabel",
                    HashPass = model.HashPass,
                    RoleId = (await _unitOfWork.Role.FindBy(role => role.Name.Equals("User")).FirstOrDefaultAsync()).Id
                });
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception e)
            {
                //add log
                return false;
            }
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _unitOfWork.User.FindBy(user => user.Email.Equals(email)).FirstOrDefaultAsync();
            if (user != null)
            {
                return new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    HashPass = user.HashPass,
                    Login = user.Login
                };
            }

            return null;
        }
    }
}
