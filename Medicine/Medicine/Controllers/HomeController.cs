using Medicine.Models;
using Medicine.Models.BusinessLogic;
using Medicine.Models.Doctors;
using Medicine.Models.Schedules;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Medicine.Controllers
{
    public class HomeController : Controller
    {
        private readonly DoctorBL doctorBl = new DoctorBL();
        public readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AdminPanel()
        {
            return View();
        }

        public ActionResult DoctorsView(int? page)
        {
            int pageSize = 12;
            int pageNumber = page ?? 1;
            List<Doctor> list = doctorBl.ListDoctors();
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult _DoctorViewPartial()
        {
            ViewBag.Doctor = new SelectValue().GetDoctor();
            ViewBag.Type = new SelectValue().GetDoctorType();
            return PartialView();
        }

        [HttpPost]
        public ActionResult _SelectedDoctorPartial(Doctor doctor)
        {

            if (doctor.DoctorId == 0 && doctor.DoctorTypeId == null)
            {
                List<Doctor> list = db.Doctors.Where(x => x.Name.Contains(doctor.Name) && x.DoctorTypeId == doctor.DoctorTypeId).ToList();
                return PartialView(list);
            }
            else if (doctor.DoctorTypeId != null && doctor.DoctorId != 0)
            {
                List<Doctor> list = db.Doctors.Where(x => x.DoctorId == doctor.DoctorId && x.DoctorTypeId == doctor.DoctorTypeId).ToList();
                return PartialView(list);
            }

            else if (doctor.DoctorTypeId == null)
            {
                List<Doctor> list = db.Doctors.Where(x => x.DoctorId == doctor.DoctorId).ToList();
                return PartialView(list);
            }
            else if (doctor.DoctorId == 0)
            {
                List<Doctor> list = db.Doctors.Where(x => x.DoctorTypeId == doctor.DoctorTypeId).ToList();
                return PartialView(list);
            }

            return PartialView(doctor);
        }

        public ActionResult ScheduleByDoctor(int id, DateTime? newDate, int? number)
        {
            if (id != 0 && newDate != null && number == 7)
            {
                // today's day 
                DateTime? date = newDate;
                int day = date.Value.Day + 1;
                int month = date.Value.Month;
                int year = date.Value.Year;
                //next day
                date = new DateTime(year, month, day);

                List<Schedule> list = doctorBl.GetScheduleById(id);
                var f = list.SkipWhile(x => x.Date.ToShortDateString() != date.Value.ToShortDateString()).GroupBy(x => x.Date).Select(x => x.First()).Take(7).ToList();
                ViewBag.Date = f;
                if (f.Count == 0)
                {
                    ViewBag.Number = f.Count;
                    ViewBag.DoctorId = id;
                    return View(list);

                }
                ViewBag.Time = f.Select(x => x.Date).Last();
                ViewBag.DoctorId = id;
                ViewBag.Number = f.Count;
                return View(list);
            }
            else
            {
                // Today's day
                DateTime todaysDay = DateTime.Now;
                int year = todaysDay.Year;
                int month = todaysDay.Month;
                int day = todaysDay.Day - 1;
                var date = new DateTime(year, month, day);
                List<Schedule> list = doctorBl.GetScheduleById(id);
                var f = list.SkipWhile(x => x.Date <= date).GroupBy(x => x.Date).Select(x => x.First()).Take(7).ToList();
                ViewBag.Date = f;
                if (ViewBag.Date.Count == 0)
                {
                    ViewBag.Time = 0;
                }
                else
                {
                    ViewBag.Time = f.Select(x => x.Date).Last();
                }
                ViewBag.DoctorId = id;
                ViewBag.Number = f.Count;
                return View(list);
            }


        }

        [HttpPost]
        public ActionResult _WorkingDayPartial(DateTime day, int id)
        {
            List<Schedule> list = db.Schedule.Where(x => x.DoctorId == id && x.Date == day && x.IsVisible).ToList();
            if (list.Count <= 0)
            {
                List<Schedule> l = db.Schedule.Where(x => x.DoctorId == id && x.Date == day && x.IsVisible).ToList();
                ViewBag.Date = 0;
                ViewBag.DoctorId = id;
                return PartialView(l);
            }
            else
            {
                if (day < DateTime.Now)
                {
                    List<Schedule> l = db.Schedule.Where(x => x.ScheduleId == 0).ToList();
                    ViewBag.Date = 0;
                    ViewBag.DoctorId = id;
                    return PartialView(l);
                }
                else
                {
                    Schedule date = db.Schedule.Where(x => x.Date == day).FirstOrDefault();
                    ViewBag.Date = date.Date;
                    ViewBag.DoctorId = id;
                    return PartialView(list);
                }
            }
        }

        #region ChangePassword
        [HttpGet]
        public ActionResult EditPassword()
        {  
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EditPassword(ChangePasswordViewModel model)
        {

            ApplicationDbContext context = new ApplicationDbContext();
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);
            var user = await UserManager.FindAsync(User.Identity.Name, model.OldPassword);

            if (!UserManager.CheckPassword(user, model.OldPassword))
            {
                ViewBag.notification = "Incorrect password.";
                return View(model);
            }
            else
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ViewBag.notification = "try again";
                    return View(model);
                }
                else
                {
                    String hashedNewPassword = UserManager.PasswordHasher.HashPassword(model.NewPassword);
                    await store.SetPasswordHashAsync(user, hashedNewPassword);
                    await store.UpdateAsync(user);
                    ViewBag.notification = "successful";
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        #endregion

        #region License

        public ActionResult License()
        {
            return View();
        }
        #endregion


    }
}