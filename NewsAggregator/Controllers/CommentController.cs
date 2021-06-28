using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Serviсes.Interfaces;
using NewsAggregator.Models.Comment;

namespace NewsAggregator.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public CommentsController(ICommentService commentService,
            IUserService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> List(Guid newsId)
        {
            var comments = await _commentService.GetCommentsByNewsId(newsId);

            return View(new CommentsListViewModel
            {
                NewsId = newsId,
                Comments = comments
            });
        }

        [HttpGet]
        public async Task<IActionResult> CreateCommentPartial()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentViewModel model)
        {
            var userClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimsIdentity.DefaultNameClaimType));
            var userLogin = userClaim?.Value; 
            var user = await _userService.GetUser(null, null ,userLogin);

            var commentDto = new CommentDto()
            {
                Id = Guid.NewGuid(),
                NewsId = model.NewsId,
                Text = model.CommentText,
                Created = DateTime.Now,
                UserId = user.Id,
                UserLogin = user.Login
            };
            await _commentService.AddComment(commentDto);

            return Ok();
        }
    }

}

