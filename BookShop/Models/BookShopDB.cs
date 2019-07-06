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
        public string Caption { get; set; }
        public string File { get; set; }
        public int PublishYear { get; set; }

      
        public int PublisherID { get; set; }

        
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
        public string Detailes { get; set; }
        public string  File { get; set; }
        public bool? IsPublish { get; set; }
        [DefaultValue("0")]
        public bool? Delete { get; set; }
        public DateTime? PublishDate { get; set; }
        [Column(TypeName = "image")]
        public byte[] Image { get; set; }


        [ForeignKey("category")]
        public int? ParentCategoryID { get; set; }

        public Category category { get; set; }
        public List<Category> categories { get; set; }
        public List<Book_Category> book_Categories { get; set; }
    }

    public class ContactMe
    {
        [Key]
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
