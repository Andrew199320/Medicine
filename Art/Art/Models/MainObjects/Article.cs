using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Art.Models.MainObjects
{
    public class Article:RecordHistory
    {
        [Key]
        public int IdArticle { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Images> Images { get; set; }
    }
}