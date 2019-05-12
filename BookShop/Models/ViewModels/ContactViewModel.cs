using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models.ViewModels
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }
        [Display(Name = "نام ")]
        [Required]

        public string Name { get; set; }
        [Display(Name = "ایمیل ")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string Email { get; set; }
        [Display(Name = "پیام")]
        [Required]

        public string Message { get; set; }
        [Display(Name = "تاریخ ارسال")]
        public DateTime DateCreate { get; set; }
    }
}
