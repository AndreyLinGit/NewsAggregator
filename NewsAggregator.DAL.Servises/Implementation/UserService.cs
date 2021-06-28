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
using System.Net.Mime;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using NewsAggregator.DAL.Serviсes.Interfaces;
using Serilog;


namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ImageStorage _imageStorage;

        public UserService(IUnitOfWork unitOfWork, IOptions<ImageStorage> imageStorage, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _imageStorage = imageStorage.Value;
        }

        public string GetPasswordHash(string modelPassword)
        {
            string specialValue = _configuration["Password:SecuritySymmetricKey"];
            var sha256 = new SHA256CryptoServiceProvider();
            var sha256data = sha256.ComputeHash(Encoding.UTF8.GetBytes(modelPassword + specialValue));
            var hashedPassword = Encoding.UTF8.GetString(sha256data);
            return hashedPassword;
        }

        public async Task<bool> RegisterUserWhitoutConfirmation(UserDto model)
        {
            try
            {
                await _unitOfWork.User.Add(_mapper.Map<User>(model));
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception e)
            {
                Log.Warning("Register was failed", e.StackTrace);
                return false;
            }
        }

        public async Task<UserDto> GetUser(Guid? id, string email, string login)
        {
            if (id != null)
            {
                return _mapper.Map<UserDto>(await _unitOfWork.User.FindBy(user => user.Id.Equals(id))
                    .FirstOrDefaultAsync());
            }
            if (email != null)
            {
                var result = _mapper.Map<UserDto>(await _unitOfWork.User.FindBy(user => user.Email.Equals(email))
                    .FirstOrDefaultAsync()); 
                if (result != null)
                {
                    return result;
                }
            }
            if (login != null)
            {
                var result = _mapper.Map<UserDto>(await _unitOfWork.User.FindBy(user => user.Login.Equals(login))
                    .FirstOrDefaultAsync());
                if (result != null)
                {
                    return result;
                }
            }
            return null;
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

            var user = await _unitOfWork.User.GetById(userId);
            if (user != null)
            {
                user.ImagePath = path;
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
                using (var fileStream = File.OpenRead(_configuration["ImageStorage:DefaultPath"]))
                {
                    var fileType = @"data:image/" + path.Substring(path.LastIndexOf(".") + 1) + @";base64,";
                    var image = new byte[fileStream.Length];
                    fileStream.Read(image, 0, image.Length);
                    return fileType + Convert.ToBase64String(image);
                }
            }
        }

        public Task<string> GetUserEmailByRefreshToken(string requestToken)
        {
            throw new NotImplementedException();
        }
    }
}
