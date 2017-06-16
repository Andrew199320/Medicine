using Medicine.Models.Pacients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Medicine.Models.Schedules
{
    public class MeetTime:RecordHistory
    {
        [Key]
        public int MeetTimeId { get; set; }
        [DataType(DataType.Time)]
        public DateTime TimeFrom { get; set; }
        [DataType(DataType.Time)]
        public DateTime TimeTo { get; set; }

        public int TimetableId { get; set; }
        public virtual Timetable Timetable { get; set; }

    }
}