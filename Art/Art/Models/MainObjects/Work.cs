using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Art.Models.MainObjects
{
    public class Work:RecordHistory
    {
        [Key]
        public int IdWork { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Images> Images { get; set; }
    }
}