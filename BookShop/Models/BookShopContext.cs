using BookShop.Mapping;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Models
{
    public class BookShopContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server =185.4.30.126,2016;uid=amtavakoli;password=H5w%2oq2;Database=PhotoDB;Trusted_Connection=True;integrated Security=false");
        }
        //@"Server=.;uid=sa;password=1;Database=PhotoDB;Trusted_Connection=True;integrated Security=true"
        //@"Server =185.4.30.126,2016;uid=amtavakoli;password=H5w%2oq2;Database=PhotoDB;Trusted_Connection=True;integrated Security=false"
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Book_CategoryMap());
            modelBuilder.Entity<Category>().HasQueryFilter(b => (bool)!b.Delete);
            modelBuilder.Entity<Category>().Property(b => b.Delete).HasDefaultValueSql("0");
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Book_Category> Book_Categories { get; set; }
        public DbSet<ContactMe> ContactMes { get; set; }
    }
}
