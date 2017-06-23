using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class PaginInfo
    {
        public int TotalItems { get; set; }     // Total amount of books
        public int ItemsPerPage { get; set; }   // Books amount per page
        public int CurrentPage { get; set; }    // Number of current page
        public int TotalPages                   // Total pages amount
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}