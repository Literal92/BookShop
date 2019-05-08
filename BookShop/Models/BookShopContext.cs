using BookShop.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Models;
using BookShop.Models.ViewModels;

namespace BookShop.Models
{
    public class BookShopContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;uid=sa;password=1;Database=BookShopDB;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Book_CategoryMap());
            modelBuilder.Entity<Book>().HasQueryFilter(b =>(bool)!b.Delete);
            modelBuilder.Entity<Book>().Property(b => b.Delete).HasDefaultValueSql("0");
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Book_Category> Book_Categories { get; set; }
        public DbQuery<ReadAllBook> ReadAllBooks { get; set; }
    }
}
