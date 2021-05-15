using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Servises.Interfaces;
using NewsAggregator.Models;

namespace NewsAggregator.Controllers
{
    public class NewsController : Controller
    {
        private readonly IRssSourseServise _rssSourseServise;

        public NewsController(IRssSourseServise rssSourseServise)
        {
            _rssSourseServise = rssSourseServise;
        }
        public async Task<IActionResult> Index()
        {
            var modelsList = new List<NewsViewModel>();
            var news = await _rssSourseServise.GetNewsFromSourse();
            foreach (var newsDto in news)
            {
                var model = new NewsViewModel()
                {
                    Article = newsDto.Article,
                    Body = newsDto.Body,
                    Id = newsDto.Id,
                    PublishTime = newsDto.PublishTime,
                    Rating = newsDto.Rating
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
