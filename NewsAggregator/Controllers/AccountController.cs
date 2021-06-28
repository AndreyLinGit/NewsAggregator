using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Serviсes.Interfaces;
using NewsAggregator.Models;
using NewsAggregator.Models.Account;

namespace NewsAggregator.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMailService _mailService;


        public AccountController(IUserService userService, IRoleService roleService, IMailService mailService)
        {
            _userService = userService;
            _roleService = roleService;
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (await _userService.GetUser(null, model.Email,null) != null)
            {
                ModelState.AddModelError("Email", "User with that email is already exist");
            }

            if (ModelState.IsValid)
            {
                var passwordHash = _userService.GetPasswordHash(model.Password);
                var newUserId = Guid.NewGuid();
                var result = await _userService.RegisterUserWhitoutConfirmation(new UserDto
                {
                    Id = newUserId,
                    Email = model.Email,
                    HashPass = passwordHash,
                    Login = model.Login,
                });
                if (result)
                {
                    var request = new MailRequest
                    {
                        UserId = newUserId.ToString(),
                        Link = @"https://localhost:44393/Account/Confirmation/",
                        Subject = "Administration from NewsAggregator",
                        ToEmail = model.Email
                    };
                    await _mailService.SendEmailAsync(request);
                    return View("WaitingConfirmation");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var passwordHash = _userService.GetPasswordHash(model.Password);
                var user = await _userService.GetUser(null, model.EmailOrLogin, model.EmailOrLogin);

                if (user != null)
                {
                    if (passwordHash.Equals(user.HashPass))
                    {
                        await Authenticate(user);
                        return RedirectToAction("Index", "News");
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "News");
        }

        private async Task Authenticate(UserDto dto)
        {
            try
            {
                const string authType = "ApplicationCookie";
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, dto.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, (await _roleService.GetUserRole(dto.Id)).Name)
                };

                var identity = new ClaimsIdentity(claims,
                    authType,
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserPage()
        {
            var userClaim =
                HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimsIdentity.DefaultNameClaimType));
            var userLogin = userClaim?.Value;
            var user = await _userService.GetUser(null, userLogin, userLogin);
            var model = new UserViewModel
            {
                Email = user.Email,
                Id = user.Id,
                Login = user.Login,
                ImagePath = await _userService.GetUserImage(user.ImagePath)
            };

            return View(model);
        }

        public async Task<IActionResult> Confirmation(Guid id)
        {
            await _roleService.AddRoleToUser(id);
            var user = await _userService.GetUser(id, null, null);
            await Authenticate(user);
            var model = new ConfirmationViewModel
            {
                Email = user.Email,
                Login = user.Login
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateImage(Guid id)
        {
            var model = new ImageModel
            {
                UserId = id
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateImage(ImageModel model)
        {
            if (ModelState.IsValid)
            {
                await _userService.SaveUserImage(new ImageDto
                {
                    ImageFile = model.ImageFile,
                    ImagePath = model.ImagePath
                }, model.UserId);
            }
            return RedirectToAction("UserPage", "Account");
        }
    }

}
