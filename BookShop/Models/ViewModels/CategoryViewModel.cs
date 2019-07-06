using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string Detailes { get; set; }
        public string File { get; set; }
        [Display(Name = " این اثر روی سایت منتشر شود ؟")]
        public bool IsPublish { get; set; }
        public DateTime PublishDate { get; set; }
        public byte[] Image { get; set; }

    }
}
