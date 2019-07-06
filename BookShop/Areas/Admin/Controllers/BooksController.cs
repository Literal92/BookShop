using BookShop.Models;
using BookShop.Models.Repository;
using BookShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BooksController : Controller
    {
        private readonly BookShopContext _context;
        private readonly BooksRepository _repository;
        public BooksController(BookShopContext context, BooksRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public IActionResult Index(int page = 1, int row = 5, string sortExpression = "Name", string title = "")
        {
            title = String.IsNullOrEmpty(title) ? "" : title;
            List<int> Rows = new List<int>
            {
                5,10,15,20,50,100
            };

            ViewBag.RowID = new SelectList(Rows, row);
            ViewBag.NumOfRow = (page - 1) * row + 1;
            ViewBag.Search = title;

            var connect = _context.ContactMes.Select(x => new ContactViewModel
            {
                Name = x.Name,
                Message = x.Message,
                Email = x.Email,
                DateCreate = x.DateCreate,
                ContactId = x.ContactId
            }).ToList();

            var PagingModel = PagingList.Create(connect, row, page, sortExpression, "Neme");
            PagingModel.RouteValue = new RouteValueDictionary
            {
                {"row",row},
                {"title",title }
            };

          

            //ViewBag.Categories = _repository.GetAllCategories();
            //ViewBag.LanguageID = new SelectList(_context.Languages, "LanguageName", "LanguageName");
            //ViewBag.PublisherID = new SelectList(_context.Publishers, "PublisherName", "PublisherName");
            ViewBag.ConnectMe = _context.ContactMes.ToList();

            return View(PagingModel);
        }


        public IActionResult Create()
        {
            ViewBag.LanguageID = new SelectList(_context.Languages, "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(_context.Publishers, "PublisherID", "PublisherName");

            return View();
        }

        public async Task<IActionResult> CreatePhoto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePhoto(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                DateTime? PublishDate = null;
                if (viewModel.IsPublish == true)
                {
                    PublishDate = DateTime.Now;
                }


                Category category = new Category()
                {
                    Delete = false,
                    CategoryName = viewModel.title,
                    IsPublish = viewModel.IsPublish,
                    Detailes = viewModel.Detailes,
                    PublishDate = PublishDate,
                    File = viewModel.File,
                };
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("CreatePhoto");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BooksCreateEditViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                List<Book_Category> categories = new List<Book_Category>();
                if (ViewModel.CategoryID != null)
                    categories = ViewModel.CategoryID.Select(a => new Book_Category { CategoryID = a }).ToList();

               

               
                Book book = new Book()
                {
                    //Delete = false,
                    LanguageID = ViewModel.LanguageID,
                    PublishYear = ViewModel.PublishYear,
                    PublisherID = ViewModel.PublisherID,
                    book_Categories = categories,
                };

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.LanguageID = new SelectList(_context.Languages, "LanguageID", "LanguageName");
                ViewBag.PublisherID = new SelectList(_context.Publishers, "PublisherID", "PublisherName");
                return View(ViewModel);
            }
        }

        public IActionResult Details(int id)
        {
            //var BookInfo = _context.ReadAllBooks.Where(b => b.BookID == id).First();

            //var BookInfo = _context.ReadAllBooks.FromSql("SELECT b.BookID, b.ISBN, b.Image, b.IsPublish, b.NumOfPages, b.Price, b.PublishDate, b.PublishYear, b.Stock, b.Summary, b.Title, b.Weight, l.LanguageName, p.PublisherName, dbo.GetAllAuthor(b.BookID) AS Authors, dbo.GetAllTranslators(b.BookID) AS Translators, dbo.GetAllCategories(b.BookID) AS Categories FROM  dbo.BookInfo AS b INNER JOIN dbo.Languages AS l ON b.LanguageID = l.LanguageID INNER JOIN dbo.Publishers AS p ON b.PublisherID = p.PublisherID WHERE(b.[Delete] = 0)")
            //    .Where(b=>b.BookID==id).First();

            var BookInfo = _context.Query<ReadAllBook>().Where(b => b.BookID == id).First();

            return View(BookInfo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var Book = _context.Books.Find(id);
            if (Book != null)
            {
                //Book.Delete = true;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            else
            {
                var Book = _context.Books.FindAsync(id);
                if(Book==null)
                {
                    return NotFound();
                }

                else
                {
                    var ViewModel = (from b in _context.Books.Include(l => l.Language)
                                     .Include(p => p.Publisher)
                                     where (b.BookID == id)
                                     select new BooksCreateEditViewModel
                                     {
                                         BookID = b.BookID,
                                         LanguageID = b.LanguageID,
                                         PublisherID = b.Publisher.PublisherID,
                                         PublishYear = b.PublishYear,

                                     }).FirstAsync();

                    int[] CategoriesArray = await (from c in _context.Book_Categories
                                                   where (c.BookID == id)
                                                   select c.CategoryID).ToArrayAsync();

                    ViewModel.Result.CategoryID = CategoriesArray;

                    ViewBag.LanguageID = new SelectList(_context.Languages, "LanguageID", "LanguageName");
                    ViewBag.PublisherID = new SelectList(_context.Publishers, "PublisherID", "PublisherName");

                    return View(await ViewModel);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BooksCreateEditViewModel ViewModel)
        {
            ViewBag.LanguageID = new SelectList(_context.Languages, "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(_context.Publishers, "PublisherID", "PublisherName");

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime? PublishDate;
                    if(ViewModel.IsPublish==true && ViewModel.RecentIsPublish==false)
                    {
                        PublishDate = DateTime.Now;
                    }
                    else if(ViewModel.RecentIsPublish==true && ViewModel.IsPublish==false)
                    {
                        PublishDate = null;
                    }

                    else
                    {
                        PublishDate = ViewModel.PublishDate;
                    }

                    Book book = new Book()
                    {
                        BookID = ViewModel.BookID,
                        LanguageID = ViewModel.LanguageID,
                        PublisherID = ViewModel.PublisherID,
                        PublishYear = ViewModel.PublishYear,
                    };

                    _context.Update(book);

                    var RecentCategories = (from c in _context.Book_Categories
                                            where (c.BookID == ViewModel.BookID)
                                            select c.CategoryID).ToArray();

                    var DeletedCategories = RecentCategories.Except(ViewModel.CategoryID);

                    var AddedCategories = ViewModel.CategoryID.Except(RecentCategories);

                    if (DeletedCategories.Count() != 0)
                        _context.RemoveRange(DeletedCategories.Select(a => new Book_Category { CategoryID = a, BookID = ViewModel.BookID }).ToList());

                    if (AddedCategories.Count() != 0)
                        _context.AddRange(AddedCategories.Select(a => new Book_Category { CategoryID = a, BookID = ViewModel.BookID }).ToList());

                    await _context.SaveChangesAsync();

                    ViewBag.MsgSuccess = "ذخیره تغییرات با موفقیت انجام شد.";
                    return View(ViewModel);
                }

                catch
                {
                    ViewBag.MsgFailed = "در ذخیره تغییرات خطایی رخ داده است.";
                    return View(ViewModel);
                }
            }

            else
            {
                ViewBag.MsgFailed = "اطلاعات فرم نامعتبر است.";
                return View(ViewModel);
            }
        }
    }
}