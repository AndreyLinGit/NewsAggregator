using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.UserCommand;
using NewsAggregator.DAL.CQRS.Queries.UserQueries;
using NewsAggregator.DAL.Serviсes.Interfaces;
using Serilog;

namespace NewsAggregator.DAL.Serviсes.Implementation.Cqrs
{
    public class UserCqrsService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ImageStorage _imageStorage;

        public UserCqrsService(IMediator mediator, IOptions<ImageStorage> imageStorage, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator;
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
                await _mediator.Send(new AddUserCommand(model));
                
                return true;
            }
            catch (Exception e)
            {
                Log.Warning("Register was failed", e.StackTrace);
                return false;
            }
        }


        public async Task SaveUserImage(ImageDto image, Guid userId) //??
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

            await _mediator.Send(new UpdateUserImageCommand(userId, path));
        }

        public async Task<string> GetUserImage(string path) //??
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

        public async Task<UserDto> GetUser(Guid? id, string email, string login)
        {
            return await _mediator.Send(new GetUserQuery(id, email, login));
        }

        public async Task<string> GetUserEmailByRefreshToken(string refreshToken)
        {
            try
            {
                var userEmail = await _mediator.Send(new GetUserEmailByRefreshTokenQuery(refreshToken));

                return userEmail;
            }
            catch (Exception e)
            {
                //Log.Error(e, "Refresh token was not successful");
                throw;
            }
        }
    }
}
