using MyWeb.Models.Common_classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class TattooStyles:RecordHistory
    {
        [Key]
        public int IdTattooStyles { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int? IdTatoos { get; set; }
        public virtual Tattoos Tattoo { get; set;  }

        public virtual ICollection<Images> Images { get; set; }
    }
}