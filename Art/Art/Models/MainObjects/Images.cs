using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Art.Models.MainObjects
{
    public class Images:RecordHistory
    {
        [Key]
        public int IdImages { get; set; }
        public string Url { get; set; }

        public int? IdWork { get; set; }
        public virtual Work Work { get; set; }

        public int? IdArticle { get; set; }
        public virtual Article Articles { get; set; }

        public int? IdCategory { get; set; }
        public virtual Category Categories { get; set; }
    }
}