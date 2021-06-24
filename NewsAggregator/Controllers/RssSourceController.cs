using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.Controllers
{
    public class RssSourceController : Controller
    {
        private readonly IRssSourceService _rssSourceService;

        public RssSourceController(IRssSourceService rssSourceService)
        {
            _rssSourceService = rssSourceService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _rssSourceService.GetAllSources();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string url)
        {
            var dto = new RssSourceDto
            {
                Id = Guid.NewGuid(),
                Name = name,
                Url = url
            };
            await _rssSourceService.AddSource(dto);
            return Redirect("/RssSource/Index");
        }

        public async Task<IActionResult> AggregateNewsFromRssSources()
        {
            await _rssSourceService.GetNewsFromSources();
            return Redirect("/News/Index");
        }
    }
}
