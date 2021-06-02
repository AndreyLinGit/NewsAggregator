using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;
using UserDto = NewsAggregator.DAL.Core.DTOs.UserDto;

namespace NewsAggregator.DAL.Servises.Interfaces
{
    public interface IUserService
    {
        string GetPasswordHash(string modelPassword);
        Task<bool> RegisterUser(UserDto model);
        Task<UserDto> GetUserByEmail(string email);
        Task<UserDto> GetUserByLogin(string email);
        Task<UserDto> GetUserById(Guid userId);
        Task Confirm(UserDto model);
    }
}