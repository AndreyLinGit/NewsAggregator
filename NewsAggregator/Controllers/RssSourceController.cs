using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Servises.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.Controllers
{
    public class RssSourceController : Controller
    {
        private readonly IRssSourceService _rssSourceService;
        private readonly INewsService _newsService;

        public RssSourceController(IRssSourceService rssSourceService, INewsService newsService)
        {
            _rssSourceService = rssSourceService;
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _rssSourceService.GetAllSources();
            //var model = new List<RssSource>();
            //model.Add(new RssSource()
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Onliner",
            //        NewsCollection = null,
            //        Url = "https://www.onliner.by/"
            //    });
            return View(model);
        }

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
            return  Redirect("/RssSource/Index");
        }

        public async Task<IActionResult> AggregateNewsFromRssSources()
        {
            var news = await _rssSourceService.GetNewsFromSource();
            var newsRange = new List<News>();
            foreach (var singleNews in news)
            {
                var newsEntity = new News
                {
                    Id = singleNews.Id,
                    Article = singleNews.Article,
                    Body = singleNews.Body,
                    PublishTime = singleNews.PublishTime,
                    Rating = singleNews.Rating, 
                    RssSourceId = singleNews.RssSourceId
                };
                newsRange.Add(newsEntity);
            }
            await _newsService.AddRangeOfNews(newsRange);
            return Redirect("/News/Index");
        }
    }
}
