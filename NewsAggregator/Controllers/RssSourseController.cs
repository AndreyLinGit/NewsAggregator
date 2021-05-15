using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Servises.Interfaces;

namespace NewsAggregator.Controllers
{
    public class RssSourseController : Controller
    {
        private readonly IRssSourseServise _rssSourseServise;

        public RssSourseController(IRssSourseServise rssSourseServise)
        {
            _rssSourseServise = rssSourseServise;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult AggregateNewsFromRssSourses()
        {
            _rssSourseServise.GetNewsFromSourse();
            return Redirect("/News/Index");
        }
    }
}
