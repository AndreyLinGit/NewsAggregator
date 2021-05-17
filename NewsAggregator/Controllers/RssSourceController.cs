using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Servises.Interfaces;

namespace NewsAggregator.Controllers
{
    public class RssSourceController : Controller
    {
        private readonly IRssSourceService _rssSourceService;

        public RssSourceController(IRssSourceService rssSourceService)
        {
            _rssSourceService = rssSourceService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult AggregateNewsFromRssSources()
        {
            _rssSourceService.GetNewsFromSourse();
            return Redirect("/News/Index");
        }
    }
}
