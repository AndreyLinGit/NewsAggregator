using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace GoodNewsAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromHeader] Guid id)
        {
            if (id != Guid.Empty)
            {
                return Ok(await _newsService.GetNewsById(id));
            }

            return BadRequest();

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromHeader] int count, string lastGottenDate)
        {
            if (count != 0 && lastGottenDate != null)
            {
                return Ok(await _newsService.GetPartOfNews(count, lastGottenDate));
            }
            return BadRequest();
        }
    }
}
