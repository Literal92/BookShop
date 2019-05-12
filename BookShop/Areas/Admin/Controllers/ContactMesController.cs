using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookShop.Models;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactMesController : Controller
    {
        private readonly BookShopContext _context;

        public ContactMesController(BookShopContext context)
        {
            _context = context;
        }

        // GET: Admin/ContactMes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactMes.ToListAsync());
        }

        // GET: Admin/ContactMes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMe = await _context.ContactMes
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contactMe == null)
            {
                return NotFound();
            }

            return View(contactMe);
        }

        // GET: Admin/ContactMes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ContactMes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,Name,Email,Message,DateCreate")] ContactMe contactMe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactMe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactMe);
        }

        // GET: Admin/ContactMes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMe = await _context.ContactMes.FindAsync(id);
            if (contactMe == null)
            {
                return NotFound();
            }
            return View(contactMe);
        }

        // POST: Admin/ContactMes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,Name,Email,Message,DateCreate")] ContactMe contactMe)
        {
            if (id != contactMe.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactMe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactMeExists(contactMe.ContactId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contactMe);
        }

        // GET: Admin/ContactMes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMe = await _context.ContactMes
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contactMe == null)
            {
                return NotFound();
            }

            return View(contactMe);
        }

        // POST: Admin/ContactMes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactMe = await _context.ContactMes.FindAsync(id);
            _context.ContactMes.Remove(contactMe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactMeExists(int id)
        {
            return _context.ContactMes.Any(e => e.ContactId == id);
        }
    }
}
