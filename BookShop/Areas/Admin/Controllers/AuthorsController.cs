using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookShop.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        private readonly BookShopContext _context;

        public AuthorsController(BookShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var Authors = _context.Authors.ToListAsync();

            ViewBag.EntityStates = DisplayStates(_context.ChangeTracker.Entries());
            return View(await Authors);
        }

        // GET: Admin/Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        public IActionResult Create()
        {
            var DisconnectedEntity = new Author() {FirstName="Arezoo",LastName="Ebrahimi",AuthorID=3};

            var EntityState = _context.Entry(DisconnectedEntity).State;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorID,FirstName,LastName")] Author author)
        {
            if (ModelState.IsValid)
            {
                //_context.Attach(author).State = EntityState.Added;
                _context.Entry(author).State = EntityState.Added;
                //_context.Add(author);

                var EntityStates= DisplayStates(_context.ChangeTracker.Entries());

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Admin/Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorID,FirstName,LastName")] Author author)
        {
            if (id != author.AuthorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(author);
                    //_context.Attach(author).State = EntityState.Modified;
                    _context.Entry(author).State = EntityState.Modified;

                    var EntityStates = DisplayStates(_context.ChangeTracker.Entries());

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorID))
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
            return View(author);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            //_context.Authors.Remove(author);

            _context.Attach(author).State = EntityState.Deleted;

            var EntityStates = DisplayStates(_context.ChangeTracker.Entries());
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorID == id);
        }


        private List<EntityStates> DisplayStates(IEnumerable<EntityEntry> entities)
        {
            List<EntityStates> EntityStates = new List<EntityStates>();
            foreach(var entry in entities)
            {
                EntityStates states = new EntityStates()
                {
                    EntityName = entry.Entity.GetType().Name,
                    EntityState=entry.State.ToString(),
                };

                EntityStates.Add(states);
            }

            return EntityStates;
        }
    }
}
