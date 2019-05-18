using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    public class FaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Biography()
        {
            return View();
        }
        public IActionResult Photos()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Project()
        {

            return View();
        }
    }
}