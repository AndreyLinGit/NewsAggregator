using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace GoodNewsAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public CommentController(ICommentService commentService, IUserService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromHeader] Guid id)
        {
            return Ok(await _commentService.GetCommentsByNewsId(id));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Guid newsId, string text)
        {
            var userClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimsIdentity.DefaultNameClaimType));
            var userLogin = userClaim?.Value;
            var user = await _userService.GetUser(null, null, userLogin);
            var commentDto = new CommentDto
            {
                Id = Guid.NewGuid(),
                NewsId = newsId,
                Text = text,
                Created = DateTime.Now,
                UserId = user.Id,
                UserLogin = user.Login
            };
            await _commentService.AddComment(commentDto);
            return Ok();
        }
    }
}
