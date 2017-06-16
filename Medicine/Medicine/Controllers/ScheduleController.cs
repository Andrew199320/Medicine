using Medicine.Models;
using Medicine.Models.BusinessLogic;
using Medicine.Models.Doctors;
using Medicine.Models.Pacients;
using Medicine.Models.Schedules;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medicine.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly ScheduleBL scheduleBL = new ScheduleBL();

        #region Schedule

        #endregion

        #region Timetable

        public ActionResult TimetableList(int?page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            ViewBag.MeetingList = scheduleBL.ListMeetTime();
            List<Timetable> list = scheduleBL.ListTimetable();
            return View(list.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult CreateTimetable()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTimetable(Timetable table)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                scheduleBL.AddTimetable(table, userName);
                return RedirectToAction("TimetableList");
            }
            return View(table);
        }
        public ActionResult EditTimetable(int id)
        {
            Timetable time = scheduleBL.GetTimetable(id);
            return View(time);
        }
        [HttpPost]
        public ActionResult EditTimetable(Timetable time)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                scheduleBL.ChangeTimetable(time, userName);
                return RedirectToAction("TimetableList");
            }
            return View(time);
        }
        public ActionResult DeleteTimetable(int id)
        {
            string userName = User.Identity.Name;
            scheduleBL.DeleteTimetable(id, userName);
            TempData["RedirectPartial"] = true;
            return RedirectToAction("TimetableList");
        }
        #endregion

        #region MeetTime

        public ActionResult CreateMeetTime(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public ActionResult CreateMeetTime(MeetTime time, int id)
        {
            ViewBag.Id = id;

            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                scheduleBL.AddMeetTime(time, userName, id);
                return RedirectToAction("TimetableList");
            }
            return View(time);
        }
        public ActionResult EditMeetTime(int id)
        {
            MeetTime time = scheduleBL.GetMeetTime(id);
            return View(time);
        }
        [HttpPost]
        public ActionResult EditMeetTime(MeetTime time)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                scheduleBL.ChangeMeetTime(time, userName);
                return RedirectToAction("TimetableList");
            }
            return View(time);
        }
        public ActionResult DeleteMeetTime(int id)
        {
            string userName = User.Identity.Name;
            scheduleBL.DeleteMeeTime(id, userName);
            TempData["RedirectPartial"] = true;
            return RedirectToAction("TimetableList");
        }

        public ActionResult ScheduleList(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            List<Doctor> list = new DoctorBL().ListDoctors();
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult WorkingJournal(int id, int?page)
        {
            int pageSize = 20;
            int pageNumber = page ?? 1 ;
            ViewBag.DoctorId = id;
            List<Schedule> schedule = scheduleBL.WorkingJournalList(id);
            return View(schedule.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult CreateJournal(int id)
        {
            ViewBag.DoctorId = id;
            ViewBag.Timetable = new SelectValue().GetTimetable();
            return PartialView();
        }
        [HttpPost]

        public ActionResult CreateJournal(Schedule schedule, int id)
        {
            ViewBag.DoctorId = id;
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                scheduleBL.AddWorkingJournal(schedule, userName, id);
                return RedirectToAction("WorkingJournal", new { id = id });
            }
            ViewBag.Timetable = new SelectValue().GetTimetable();
            return PartialView(schedule);
        }
        #endregion

        #region RegistrationToAppointmant
        public ActionResult AppointmentForm()
        {
            return View();
        }
        public ActionResult RegisterForm(DateTime? appointmantDate, string doctorName)
        {
            List<Schedule> list = (from d in db.Schedule
                                   where (d.Doctor.Name + d.Doctor.Sername).Contains(doctorName) && d.Date == appointmantDate
                                   select d).ToList();
            return PartialView(list);

        }
        public ActionResult RegisterToAnAppointmant(int id)
        {
            Schedule schedule = db.Schedule.Where(x => x.ScheduleId == id).FirstOrDefault();
            return View(schedule);
        }
        [HttpPost]
        public ActionResult RegisterToAnAppointmant(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                Pacient currentUser = db.Pacients.Where(x => x.UserName == userName).FirstOrDefault();
                int Id = currentUser.PacientId;
                scheduleBL.RegisterAppointment(schedule, Id);
                return RedirectToAction("AppointmentForm");
            }
            return View(schedule);
        }
        #endregion

        #region AppointmantRegistration
        public ActionResult AppointmantRegistration(int id)
        {
            Schedule app = db.Schedule.Where(x => x.IsVisible && x.ScheduleId == id).FirstOrDefault();
            return View(app);
        }
        [HttpPost]
        public ActionResult AppointmantRegistration(Schedule schedule)
        {
            string userName = User.Identity.Name;
            Pacient pacient = db.Pacients.Where(x => x.UserName == userName).FirstOrDefault();
            if (ModelState.IsValid)
            {
                schedule.isBooked = true;
                schedule.PacientId = pacient.PacientId;
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
           
            return View(schedule);
        }
        #endregion

        #region AppontmantsTime

        public ActionResult AppointmantsTime(DateTime date, int id )
        {
            List<Schedule> list = db.Schedule.Where(x => x.Date == date && x.DoctorId == id).ToList();
            ViewBag.Date = date;
            return View(list);
        }
        #endregion

    }
}