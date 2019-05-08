using BookShop.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.Models.Repository
{
    public class BooksRepository
    {
        private readonly BookShopContext _context;
        public BooksRepository(BookShopContext context)
        {
            _context = context;
        }


        public List<TreeViewCategory> GetAllCategories()
        {
            //var Categories = (from c in _context.Categories
            //                  where (c.ParentCategoryID == null)
            //                  select new TreeViewCategory { id = c.CategoryID, title = c.CategoryName }).ToList();
            //foreach (var item in Categories)
            //{
            //    BindSubCategories(item);
            //}

            return null;
        }

        public void BindSubCategories(TreeViewCategory category)
        {
            var SubCategories = (from c in _context.Categories
                                 where (c.ParentCategoryID == category.id)
                                 select new TreeViewCategory { id = c.CategoryID, title = c.CategoryName }).ToList();
            foreach (var item in SubCategories)
            {
                BindSubCategories(item);
                category.subs.Add(item);
            }
        }

        public List<BooksIndexViewModel> GetAllBooks(string title)
        {
            string CategotyName = "";
            List<BooksIndexViewModel> ViewModel = new List<BooksIndexViewModel>();
            //var Books = _context.Books.Where(x => x.Title == title);

            //foreach (var item in Books)
            //{
            //    CategotyName = "";
            //    BooksIndexViewModel VM = new BooksIndexViewModel()
            //    {
            //        BookID = item.BookID,
            //        ISBN = item.ISBN,
            //        Title = item.Title,
            //        Price = item.Price,
            //        IsPublish = item.IsPublish,
            //        PublishDate = item.PublishDate,
            //        Stock = item.Stock,
            //        Category = CategotyName,
            //    };

            //    ViewModel.Add(VM);
            //}

            return ViewModel;
        }
    }
}
