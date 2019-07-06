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
