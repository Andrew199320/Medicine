using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medicine.Models.Schedules
{
    public class Timetable:RecordHistory
    {
        public int TimetableId { get; set; }
        public string Name { get; set; }
    }
}