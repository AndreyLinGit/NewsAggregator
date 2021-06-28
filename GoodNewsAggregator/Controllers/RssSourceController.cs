using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Serviсes.Interfaces;
using NewsAggregator.DAL.Core.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoodNewsAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssSourceController : ControllerBase
    {
        private readonly IRssSourceService _rssSourceService;

        public RssSourceController(IRssSourceService rssSourceService)
        {
            _rssSourceService = rssSourceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _rssSourceService.GetAllSources());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string name, string url)
        {
            var dto = new RssSourceDto
            {
                Id = Guid.NewGuid(),
                Name = name,
                Url = url
            };
            await _rssSourceService.AddSource(dto);
            return Ok();
        }
    }
}
