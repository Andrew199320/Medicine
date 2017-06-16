using Medicine.Models.Doctors;
using Medicine.Models.Pacients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medicine.Models.Schedules
{
    public class Schedule : RecordHistory
    {
        [Key]
        public int ScheduleId { get; set; }

        public int? DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        public int? TimetableId { get; set; }
        public virtual Timetable Timetable { get; set; }

        public int? PacientId { get; set; }
        public virtual Pacient Pacient { get; set; }

        public DateTime Date { get; set; }

        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }

        public bool? isBooked { get; set; }

    }
}