using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Servises.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;
using NewsAggregator.Models;

namespace NewsAggregator.Controllers
{
    public class NewsController : Controller
    {
        //For work without database
        private readonly IRssSourceService _rssSourceService;
        private bool isCostil = false;
        //DELETE IF AFTER!

        private readonly INewsService _newsService;

        public NewsController(INewsService newsService, IRssSourceService rssSourceService)
        {
            _newsService = newsService;
            _rssSourceService = rssSourceService;
        }


        public async Task<IActionResult> Index()
        {
            //var newsList;
            if (isCostil)
            {
                //newsList = await _rssSourceService.GetNewsFromSource(isCostil);
            }
            else
            {
                 
            }
            var newsList = await _newsService.GetAllNews(); //Think about "Get()"
            var modelsList = new List<NewsViewModel>();
            foreach (var news in newsList)
            {
                var model = new NewsViewModel
                {
                    Id = news.Id,
                    Article = news.Article,
                    Body = news.Body,
                    PublishTime = news.PublishTime,
                    Rating = news.Rating
                };
                modelsList.Add(model);
            }
            return View(modelsList);
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
