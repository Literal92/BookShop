using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    [Table("Photo")]
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string File { get; set; }
        public short Weight { get; set; }
        public string ISBN { get; set; }
        public bool? IsPublish { get; set; }
        public DateTime? PublishDate { get; set; }
        public int PublishYear { get; set; }

        [DefaultValue("0")]
        public bool? Delete { get; set; }
        public int PublisherID { get; set; }

        [Column(TypeName ="image")]
        public byte[] Image { get; set; }
        public int LanguageID { get; set; }
        public Language Language { get; set; }
        public List<Book_Category> book_Categories { get; set; }
        public Publisher Publisher { get; set; }
    }

    public class Book_Category
    {
        public int BookID { get; set; }
        public int CategoryID { get; set; }

        public Book Book { get; set; }
        public Category Category { get; set; }
    }


    public class Publisher
    {
        [Key]
        public int PublisherID { get; set; }
        public string PublisherName { get; set; }

        public List<Book> Books { get; set; }
    }



    public class Language
    {
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }

        public List<Book> Books { get; set; }
    }

    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        [ForeignKey("category")]
        public int? ParentCategoryID { get; set; }

        public Category category { get; set; }
        public List<Category> categories { get; set; }
        public List<Book_Category> book_Categories { get; set; }
    }
}
