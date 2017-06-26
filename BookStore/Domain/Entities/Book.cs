using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Book
    {
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "ID")]
        public int BookId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Display(Name = "Author name")]
        [Required(ErrorMessage = "Author name is required.")]
        public string Author { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Book's description")]
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Display(Name = "Genre")]
        [Required(ErrorMessage = "Genre is required.")]
        public string Genre { get; set; }

        [Display(Name = "Price ($)")]
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01,double.MaxValue, ErrorMessage = "Please enter non-negative number.")]
        public decimal Price { get; set; }
    }
}
