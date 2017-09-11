using MyWeb.Models.Common_classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class Tattoos:RecordHistory
    {
        [Key]
        public int IdTatoos { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Like { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<Illustrators> Illustrators { get; set; }
        public virtual ICollection<Categories> Categories { get; set; }
        public virtual ICollection<TattooStyles> Styles { get; set; }
        public virtual ICollection<Images> Images { get; set; }
        


    }
}