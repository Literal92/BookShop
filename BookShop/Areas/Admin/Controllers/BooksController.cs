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

        public IActionResult Index(int page = 1, int row = 5, string sortExpression = "Title", string title = "")
        {
            title = String.IsNullOrEmpty(title) ? "" : title;
            List<int> Rows = new List<int>
            {
                5,10,15,20,50,100
            };

            ViewBag.RowID = new SelectList(Rows, row);
            ViewBag.NumOfRow = (page - 1) * row + 1;
            ViewBag.Search = title;

            //var PagingModel = PagingList.Create(_repository.GetAllBooks(title), row, page, sortExpression, "Title");
            //PagingModel.RouteValue = new RouteValueDictionary
            //{
            //    {"row",row},
            //    {"title",title }
            //};

            ViewBag.Categories = _repository.GetAllCategories();
            ViewBag.LanguageID = new SelectList(_context.Languages, "LanguageName", "LanguageName");
            ViewBag.PublisherID = new SelectList(_context.Publishers, "PublisherName", "PublisherName");

            return View();
        }


        public IActionResult AdvancedSearch(BooksAdvancedSearch ViewModel)
        {
            ViewModel.Title = String.IsNullOrEmpty(ViewModel.Title) ? "" : ViewModel.Title;
            ViewModel.ISBN = String.IsNullOrEmpty(ViewModel.ISBN) ? "" : ViewModel.ISBN;
            ViewModel.Publisher = String.IsNullOrEmpty(ViewModel.Publisher) ? "" : ViewModel.Publisher;
            ViewModel.Author = String.IsNullOrEmpty(ViewModel.Author) ? "" : ViewModel.Author;
            ViewModel.Translator = String.IsNullOrEmpty(ViewModel.Translator) ? "" : ViewModel.Translator;
            ViewModel.Category = String.IsNullOrEmpty(ViewModel.Category) ? "" : ViewModel.Category;
            ViewModel.Language = String.IsNullOrEmpty(ViewModel.Language) ? "" : ViewModel.Language;
            var Books = _repository.GetAllBooks(ViewModel.Title);
            return View(Books);
        }

        public IActionResult Create()
        {
            ViewBag.LanguageID = new SelectList(_context.Languages, "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(_context.Publishers, "PublisherID", "PublisherName");

            BooksSubCategoriesViewModel SubCategoriesVM = new BooksSubCategoriesViewModel(_repository.GetAllCategories(),null);
            BooksCreateEditViewModel ViewModel = new BooksCreateEditViewModel(SubCategoriesVM);
            return View(ViewModel);
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

                DateTime? PublishDate = null;

                if (ViewModel.IsPublish == true)
                {
                    PublishDate = DateTime.Now;
                }
                Book book = new Book()
                {
                    //Delete = false,
                    ISBN = ViewModel.ISBN,
                    IsPublish = ViewModel.IsPublish,
                    Stock = ViewModel.Stock,
                    Price = ViewModel.Price,
                    LanguageID = ViewModel.LanguageID,
                    Summary = ViewModel.Summary,
                    Title = ViewModel.Title,
                    PublishYear = ViewModel.PublishYear,
                    PublishDate = PublishDate,
                    Weight = ViewModel.Weight,
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
                ViewModel.Categories = _repository.GetAllCategories();
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
                Book.Delete = true;
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
                                         Title = b.Title,
                                         ISBN = b.ISBN,
                                         Price = b.Price,
                                         Stock = b.Stock,
                                         IsPublish = (bool)b.IsPublish,
                                         LanguageID = b.LanguageID,
                                         PublisherID = b.Publisher.PublisherID,
                                         PublishYear = b.PublishYear,
                                         Summary = b.Summary,
                                         Weight = b.Weight,
                                         RecentIsPublish=(bool)b.IsPublish,
                                         PublishDate=b.PublishDate,

                                     }).FirstAsync();

                    int[] CategoriesArray = await (from c in _context.Book_Categories
                                                   where (c.BookID == id)
                                                   select c.CategoryID).ToArrayAsync();

                    ViewModel.Result.CategoryID = CategoriesArray;

                    ViewBag.LanguageID = new SelectList(_context.Languages, "LanguageID", "LanguageName");
                    ViewBag.PublisherID = new SelectList(_context.Publishers, "PublisherID", "PublisherName");
                    ViewModel.Result.SubCategoriesVM = new BooksSubCategoriesViewModel(_repository.GetAllCategories(), CategoriesArray);

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
            ViewModel.SubCategoriesVM = new BooksSubCategoriesViewModel(_repository.GetAllCategories(), ViewModel.CategoryID);

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
                        Title = ViewModel.Title,
                        ISBN = ViewModel.ISBN,
                        Price = ViewModel.Price,
                        Stock = ViewModel.Stock,
                        IsPublish = ViewModel.IsPublish,
                        LanguageID = ViewModel.LanguageID,
                        PublisherID = ViewModel.PublisherID,
                        PublishYear = ViewModel.PublishYear,
                        Summary = ViewModel.Summary,
                        Weight = ViewModel.Weight,
                        PublishDate=PublishDate,
                        Delete=false,
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