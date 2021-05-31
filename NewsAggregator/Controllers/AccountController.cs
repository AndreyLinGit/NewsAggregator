using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Servises.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;
using NewsAggregator.Models.Account;

namespace NewsAggregator.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;


        public AccountController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
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
            if (await _userService.GetUserByEmail(model.Email) != null)
            {
                ModelState.AddModelError("Email","User with that email is already exist");
            }
            if (ModelState.IsValid)
            {
                var passwordHash = _userService.GetPasswordHash(model.Password);

                var result = await _userService.RegisterUser(new UserDto
                {
                    Id = Guid.NewGuid(),
                    Email = model.Email,
                    HashPass = passwordHash,
                    Login = "Mabel"
                });
                if (result)
                {
                    return RedirectToAction("Index", "News");
                }

                return BadRequest(model);
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
                var user = await _userService.GetUserByEmail(model.Email);
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
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        private async Task Authenticate(UserDto dto)
        {
            try
            {
                const string authType = "ApplicationCookie";
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, dto.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, (await _roleService.GetUserRole(dto.Email)).Name)
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
            //Claim & ClaimsIdentity & ClaimsPrinciple

        }

        public IActionResult Login2()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login2(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var passwordHash = _userService.GetPasswordHash(model.Password);
                var user = await _userService.GetUserByEmail(model.Email);
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
        public async Task<IActionResult> ForgotPass() //?
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPass(LoginViewModel model) //?
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UserPage() //?
        {
            return View();
        }
    }
}
