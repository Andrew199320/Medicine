using Medicine.Models.Pacients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Medicine.Models.Schedules
{
    public class Registration:RecordHistory
    {
        [Key]
        public int RegistrationId { get; set; }
        public DateTime Time { get; set; }
        public bool IsBooked { get; set; }

        public int? PacientId { get; set; }
        public virtual Pacient Pacient { get; set; }

        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}