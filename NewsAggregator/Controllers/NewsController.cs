using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Servises.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;
using NewsAggregator.Models;
using NewsAggregator.Models.Comment;

namespace NewsAggregator.Controllers
{
    public class NewsController : Controller
    {
        public List<NewsViewModel> result = new List<NewsViewModel>();
        //For work without database
        private readonly IRssSourceService _rssSourceService;

        private bool isCostil = true;
        //DELETE IF AFTER!

        private readonly INewsService _newsService;
        private readonly ICommentService _commentService;

        public NewsController(INewsService newsService, IRssSourceService rssSourceService,
            ICommentService commentService)
        {
            _newsService = newsService;
            _rssSourceService = rssSourceService;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            //var newsList = await _rssSourceService.GetNewsFromSource(isCostil); // (It's for work from work!)
            //var newsList = await _newsService.GetAllNews(); //Think about "Get()" (It's for work from home!) CREATE PAGINATION AND CHANGE IT!
            //var modelsList = new List<NewsViewModel>();
            //foreach (var news in newsList)
            //{
            //    var model = new NewsViewModel
            //    {
            //        Id = news.Id,
            //        Article = news.Article,
            //        Summary = news.Summary,
            //        Body = news.Body,
            //        PublishTime = news.PublishTime,
            //        Rating = news.Rating
            //    };
            //    modelsList.Add(model);
            //}

            //result = modelsList.OrderBy(x => x.PublishTime).ToList();

            //var resultModel = new List<NewsViewModel>();
            //if (modelsList.Any())
            //{
            //    resultModel = (await GetPartOfNewsFromWork(30, DateTime.Now.ToString())).ToList();
            //}

            //return View(resultModel);

            //THAT IS A REAL CODE!!!

            var newsList = await _newsService.GetPartOfNews(30, DateTime.Now.ToString());
            var modelsList = new List<NewsViewModel>();
            foreach (var news in newsList)
            {
                var model = new NewsViewModel
                {
                    Id = news.Id,
                    Article = news.Article,
                    Summary = news.Summary,
                    Body = news.Body,
                    PublishTime = news.PublishTime,
                    Rating = news.Rating,
                    SourceName = news.RssSourceName
                };
                modelsList.Add(model);
            }
            return View(modelsList);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var news = await _newsService.GetNewsById(id);
            var comments = await _commentService.GetCommentsByNewsId(id);
            var model = new NewsDetailModel
            {
                Article = news.Article,
                Body = news.Body,
                Id = news.Id,
                PublishTime = news.PublishTime,
                Rating = news.Rating,
                Comments = comments
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> InfinityPaggination(string lastGottenDate)
        {
            //var result = await GetPartOfNewsFromWork(30, lastGottenDate);
            var result = await _newsService.GetPartOfNews(30, lastGottenDate);
            var modelsList = new List<NewsViewModel>();
            foreach (var news in result)
            {
                var model = new NewsViewModel
                {
                    Id = news.Id,
                    Article = news.Article,
                    Summary = news.Summary,
                    Body = news.Body,
                    PublishTime = news.PublishTime,
                    Rating = news.Rating,
                    SourceName = news.RssSourceName
                };
                modelsList.Add(model);
            }
            return View(modelsList);
        }

        private async Task<List<NewsViewModel>> GetPartOfNewsFromWork(int count, string lastGottenPublishTime)
        {
            var date = DateTime.Parse(lastGottenPublishTime);
            var newsFromList = result.Where(news => news.PublishTime.CompareTo(date) < 0)
                .Take(count)
                .ToList();

            return newsFromList;
        }
    }
}

