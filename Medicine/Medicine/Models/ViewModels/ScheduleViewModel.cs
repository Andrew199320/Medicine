using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medicine.Models.ViewModels
{
    public class ScheduleViewModel
    {
        public int ScheduleId { get; set; }
        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }
        public bool? isBooked { get; set; }

    }
}