using MyWeb.Models.Common_classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class Illustrators:RecordHistory
    {
        [Key]
        public int IdIllustator { get; set; }
        public string Name { get; set; }
        public string Sername { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }

        public int? IdTatoos { get; set; }
        public virtual Tattoos Tattoo { get; set; }

        public string UserId { get; set; }



    }
}