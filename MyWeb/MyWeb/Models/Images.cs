using MyWeb.Models.Common_classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class Images:RecordHistory
    {
        [Key]
        public int IdImage { get; set; }
        public string Url { get; set; }

        public int? IdCategory { get; set; }
        public virtual Categories Categories { get; set; }

        public int? IdTattooStyles { get; set; }
        public virtual TattooStyles TattooStyles { get; set; }

        public int? IdTatoos { get; set; }
        public virtual Tattoos Tattoos { get; set; }
    } 
}