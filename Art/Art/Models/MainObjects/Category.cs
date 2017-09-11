using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Art.Models.MainObjects
{
    public class Category:RecordHistory
    {
        [Key]
        public int IdCategory { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int? IdWork { get; set; }
        public virtual Work Work { get; set; }

        public virtual ICollection<Images> Images { get; set; }

    }
}