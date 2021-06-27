using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace GoodNewsAggregator.Auth
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;

        public JwtAuthManager(IConfiguration configuration, IRefreshTokenService refreshTokenService,
            IUserService userService)
        {
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
            _userService = userService;
        }

        public async Task<JwtAuthResult> GenerateTokens(string email, Claim[] claims)
        {
            var jwtToken = new JwtSecurityToken("GoodNesAggregator",
                "GoodNesAggregator",
                claims,
                expires: DateTime.Now.AddMinutes(1), //from config
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var userId = (await _userService.GetUser(null,email,null)).Id;

            var refreshToken = await _refreshTokenService.GenerateRefreshToken(userId);

            return new JwtAuthResult()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }

    public class JwtAuthResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshToken
    {
        public string Token { get; set; }
    }


    public interface IJwtAuthManager
    {
        Task<JwtAuthResult> GenerateTokens(string email, Claim[] claims);
    }
}
