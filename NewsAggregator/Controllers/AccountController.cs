﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Register(RegisterViewModel model) //???? About mail's
        {
            //if (await _userService.GetUserByEmail(model.Email) != null)
            //{
            //    ModelState.AddModelError("Email","User with that email is already exist");
            //}
            if (ModelState.IsValid)
            {
                //var passwordHash = _userService.GetPasswordHash(model.Password);

                //var result = await _userService.RegisterUser(new UserDto
                //{
                //    Id = Guid.NewGuid(),
                //    Email = model.Email,
                //    HashPass = passwordHash,
                //    Login = model.Login,
                //    IsConfirmed = false
                //});
                if (true)
                {
                    var request = new MailRequest
                    {
                        Link = @"",
                        Subject = "Administration from NewsAggregator",
                        ToEmail = model.Email
                    };
                    await _mailService.SendEmailAsync(request);
                    return View("WaitingConfirmation");
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
        public async Task<IActionResult> Login(LoginViewModel model)// Think about 
        {
            if (ModelState.IsValid)
            {
                var passwordHash = _userService.GetPasswordHash(model.Password);
                var user = await _userService.GetUserByEmail(model.EmailOrLogin) != null
                    ? await _userService.GetUserByEmail(model.EmailOrLogin)
                    : await _userService.GetUserByLogin(model.EmailOrLogin);

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
                    new Claim(ClaimsIdentity.DefaultNameClaimType, dto.Login),
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