using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Servises.Interfaces;
using System.Net.Mime;


namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ImageStorage _imageStorage;

        public UserService(IUnitOfWork unitOfWork, IOptions<ImageStorage> imageStorage)
        {
            _unitOfWork = unitOfWork;
            _imageStorage = imageStorage.Value;
        }

        public string GetPasswordHash(string modelPassword)
        {
            const string specialValue = "123123123132";
            var sha256 = new SHA256CryptoServiceProvider();
            var sha256data = sha256.ComputeHash(Encoding.UTF8.GetBytes(modelPassword));
            var hashedPassword = Encoding.UTF8.GetString(sha256data);
            return hashedPassword;
        }

        public async Task<bool> RegisterUserWhitoutConfirmation(UserDto model)
        {
            try
            {
                await _unitOfWork.User.Add(new User()
                {
                    Id = model.Id,
                    Email = model.Email,
                    Login = model.Login,
                    HashPass = model.HashPass
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
                    Login = user.Login,
                    ImagePath = user.ImagePath
                };
            }

            return null;
        }

        public async Task<UserDto> GetUserByLogin(string login)
        {
            var user = await _unitOfWork.User.FindBy(user => user.Login.Equals(login)).FirstOrDefaultAsync();
            if (user != null)
            {
                return new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    HashPass = user.HashPass,
                    Login = user.Login,
                    RoleId = user.RoleId,
                    ImagePath = user.ImagePath
                };
            }

            return null;
        }

        public async Task<UserDto> GetUserById(Guid userId)
        {
            var user = await _unitOfWork.User.GetById(userId);
            return new UserDto
            {
                Email = user.Email,
                HashPass = user.HashPass,
                Id = user.Id,
                Login = user.Login,
                RoleId = user.RoleId,
                ImagePath = user.ImagePath
            };
        }

        public async Task SaveUserImage(ImageDto image, Guid userId)
        {
            if (!Directory.Exists(_imageStorage.Path))
            {
                Directory.CreateDirectory(_imageStorage.Path);
            }

            var path = Path.Combine(_imageStorage.Path
                                       + "/" 
                                       + Path.GetFileNameWithoutExtension(image.ImageFile.FileName) 
                                       + DateTime.Now.ToString("yymmssfff") 
                                       + Path.GetExtension(image.ImageFile.FileName));
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await image.ImageFile.CopyToAsync(fileStream);
            }

            var user = await _unitOfWork.User.GetById(userId); //Reform to update method 
            if (user != null)
            {
                user.ImagePath = path;
                _unitOfWork.User.Update(user);
                await _unitOfWork.SaveChangeAsync();
            }
        }

        public async Task<string> GetUserImage(string path)
        {
            if (File.Exists(path))
            {
                using (var fileStream = File.OpenRead(path))
                {
                    var fileType = @"data:image/" + path.Substring(path.LastIndexOf(".") + 1) + @";base64,";
                    var image = new byte[fileStream.Length];
                    fileStream.Read(image, 0, image.Length);
                    return fileType + Convert.ToBase64String(image);
                }
            }
            else
            {
                var defaultImage = ""; //Add Default Image
                return defaultImage;
            }
            
        }
    }
}
