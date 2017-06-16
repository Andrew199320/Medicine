using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medicine.Models
{
    public class RecordHistory
    {
        public bool IsVisible { get; set; }
        public string WhoAdded { get; set; }
        public string WhoModified { get; set; }
        public string WhoDeleted { get; set; }
        public DateTime? WhenAdded { get; set; }
        public DateTime? WhenModified { get; set; }
        public DateTime? WhenDeleted { get; set; }
        
    }
}