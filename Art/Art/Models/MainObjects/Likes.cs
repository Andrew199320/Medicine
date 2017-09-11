using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Art.Models.MainObjects
{
    public class Likes:RecordHistory
    {
        [Key]
        public int IdLike { get; set; }
        public int Number { get; set; }
        public bool Checked { get; set; }
        
        
    }
}