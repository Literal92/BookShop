using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookShop.Models;

namespace BookShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookShopContext _context;

        public HomeController(BookShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Project()
        {

            return View();
        }

        public IActionResult Contact()
        {

            return View();
        }

        public IActionResult Photos()
        {
            return View();
        }

        public IActionResult Biography()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConnect([Bind("ContactId,Name,Email,Message,DateCreate")] ContactMe contactMe)
        {
            if (ModelState.IsValid)
            {
                contactMe.DateCreate = DateTime.Now;
                _context.Add(contactMe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactMe);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
