using System;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;
using UserDto = NewsAggregator.DAL.Core.DTOs.UserDto;

namespace NewsAggregator.DAL.Serviсes.Interfaces
{
    public interface IUserService
    {
        string GetPasswordHash(string modelPassword);
        Task<bool> RegisterUserWhitoutConfirmation(UserDto model);
        Task<UserDto> GetUser(Guid? id, string email, string login);
        //Task<UserDto> GetUserByEmail(string email);
        //Task<UserDto> GetUserByLogin(string email);
        //Task<UserDto> GetUserById(Guid userId);
        Task SaveUserImage(ImageDto image, Guid userId);
        Task<string> GetUserImage(string path);
        Task<string> GetUserEmailByRefreshToken(string requestToken);
    }
}