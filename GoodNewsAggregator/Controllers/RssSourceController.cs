using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Serviсes.Interfaces;

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

        // GET: api/<RssSourceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RssSourceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RssSourceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RssSourceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RssSourceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
