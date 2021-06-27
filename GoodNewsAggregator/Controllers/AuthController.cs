using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Auth;
using GoodNewsAggregator.Requests;
using Microsoft.AspNetCore.Authorization;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace GoodNewsAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(IJwtAuthManager jwtAuthManager, IUserService userService, IRoleService roleService, IRefreshTokenService refreshTokenService)
        {
            _jwtAuthManager = jwtAuthManager;
            _userService = userService;
            _roleService = roleService;
            _refreshTokenService = refreshTokenService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (await _userService.GetUser(null, request.Email, request.Login) != null)
                {
                    return BadRequest("User with this email already existed");
                }

                var passwordHash = _userService.GetPasswordHash(request.Password);
                var newUserId = Guid.NewGuid();
                var isRegistrationSucceed = await _userService.RegisterUserWhitoutConfirmation(new UserDto()
                {
                    Id = newUserId,
                    Email = request.Email,
                    Login = request.Login,
                    HashPass = passwordHash
                });

                if (isRegistrationSucceed)
                {
                    var mailRequest = new MailRequest
                    {
                        UserId = newUserId.ToString(),
                        Subject = "Administration from NewsAggregator",
                        ToEmail = request.Email
                    };

                    return Ok(mailRequest);
                }

                return BadRequest("Unsuccessful registration");
            }
            catch (Exception e)
            {
                //Log
                return StatusCode(500, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userService.GetUser(null, request.EmailOrLogin, request.EmailOrLogin);
                if (user == null)
                {
                    return BadRequest("No user");
                }

                var passwordHash = _userService.GetPasswordHash(request.Password);
                if (passwordHash.Equals(user.HashPass))
                {
                    var jwtAuthResult = await GetJwt(user.Email);
                    return Ok(jwtAuthResult);
                }

                return BadRequest("Unsuccessful registration");
            }
            catch (Exception e)
            {
                //Log
                return StatusCode(500, e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (!await _refreshTokenService.CheckIsRefreshTokenIsValid(request.Token))
            {
                return BadRequest("Invalid Refresh Token");
            }

            var userEmail = await _userService.GetUserEmailByRefreshToken(request.Token);
            if (!string.IsNullOrEmpty(userEmail))
            {
                var jwtAuthResult = await GetJwt(userEmail);
                return Ok(jwtAuthResult);
            }

            return BadRequest("Email or password is incorrect");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Confirmation")]
        public async Task<IActionResult> Confirmation([FromHeader] Guid id)
        {
            try
            {
                await _roleService.AddRoleToUser(id);
                return Ok();
            }
            catch (Exception e)
            {
                //Log
                return StatusCode(500, e.Message);
            }
        }

        private async Task<JwtAuthResult> GetJwt(string email)
        {
            JwtAuthResult jwtResult;
            var user = await _userService.GetUser(null, email, null);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, (await _roleService.GetUserRole(user.RoleId.Value)).Name)
            };

            jwtResult = await _jwtAuthManager.GenerateTokens(user.Email, claims);
            return jwtResult;
        }
    }
}
