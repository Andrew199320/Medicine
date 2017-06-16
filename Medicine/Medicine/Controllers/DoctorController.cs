using Medicine.Models;
using Medicine.Models.BusinessLogic;
using Medicine.Models.Doctors;
using Medicine.Models.Schedules;
using PagedList;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medicine.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly DoctorBL doctorBl = new DoctorBL();

        #region Doctor 
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DoctorList(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            List<Doctor> list = doctorBl.ListDoctors();
            return View(list.ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateDoctor(string email)
        {
            SelectList list = new SelectList(
                 new List<SelectListItem>
                 {
                    new SelectListItem {Value="1", Text = "Male" },
                    new SelectListItem {Value="2", Text ="Female" }
                 }, "Value", "Text");
            ViewBag.Sex = list;
            ViewBag.Type = new SelectValue().GetDoctorType();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateDoctor(Doctor doctor)
        {
            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                try
                {
                    doctorBl.AddDoctor(doctor, userName);
                    db.SaveChanges();
                    foreach (string fileName in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[fileName];
                        if (file != null)
                        {
                            var name = DateTime.Now.ToFileTime() + Path.GetFileName(file.FileName);
                            if (file.ContentLength > 0)
                            {
                                // лучше оставить слеши Content/
                                if (!Directory.Exists(Server.MapPath("~/Content/Images/DifferentPictures/")))
                                {   // лучше оставить слеши Content/
                                    Directory.CreateDirectory(Server.MapPath("~/Content/Images/DifferentPictures/"));
                                }
                                // после Content можно не ставить / слэш
                                string path = Path.Combine(Server.MapPath("~/Content/Images/DifferentPictures") + "/" + name);
                                file.SaveAs(path);
                                // извлекаеться картинка +Content/ - слэш как разделитель
                                doctor.Photo = "/Content/Images/DifferentPictures/" + name;
                                db.Entry(doctor).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return View(doctor);
                }
            }
            return RedirectToAction("DoctorList");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditDoctor(int id)
        {
            SelectList list = new SelectList(
                 new List<SelectListItem>
                 {
                    new SelectListItem {Value="1", Text = "Male" },
                    new SelectListItem {Value="2", Text ="Female" }
                 }, "Value", "Text");
            ViewBag.Sex = list;
            Doctor doctor = doctorBl.GetDoctor(id);
            ViewBag.Type = new SelectValue().GetDoctorType();
            TempData["UrlPictue"] = doctor.Photo;
            return View(doctor);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditDoctor(Doctor doctor)
        {   string userName = User.Identity.Name;
            string url = (string)TempData["UrlPictue"];
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                if (file != null)
                {
                    var name = DateTime.Now.ToFileTime() + Path.GetFileName(file.FileName);
                    if (file.ContentLength > 0)
                    {
                        // лучше оставить слеши Content/
                        if (!Directory.Exists(Server.MapPath("~/Content/Images/DifferentPictures/")))
                        {   // лучше оставить слеши Content/
                            Directory.CreateDirectory(Server.MapPath("~/Content/Images/DifferentPictures/"));
                        }
                        // после Content можно не ставить / слэш
                        string path = Path.Combine(Server.MapPath("~/Content/Images/DifferentPictures") + "/" + name);
                        file.SaveAs(path);

                        if (url != doctor.Photo)
                        {
                            if (doctor.Sex == "1")
                            {
                                doctor.Sex = "Male";
                            }
                            else
                            {
                                doctor.Sex = "Female";
                            }
                            doctor.WhoModified = userName;
                            doctor.WhenModified = DateTime.Now;
                            doctor.Photo = "/Content/Images/DifferentPictures/" + name;
                            ViewBag.Type = new SelectValue().GetDoctorType();
                            db.Entry(doctor).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            if (doctor.Sex == "1")
                            {
                                doctor.Sex = "Male";
                            }
                            else
                            {
                                doctor.Sex = "Female";
                            }
                            doctor.WhoModified = userName;
                            doctor.WhenModified = DateTime.Now;
                            doctor.Photo = url;
                            db.Entry(doctor).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        return RedirectToAction("DoctorList");
                    }
                }
                if (doctor.Sex == "1")
                {
                    doctor.Sex = "Male";
                }
                else
                {
                    doctor.Sex = "Female";
                }
                doctor.WhoModified = userName;
                doctor.WhenModified = DateTime.Now;
                doctor.Photo = url;
                ViewBag.Type = new SelectValue().GetDoctorType();
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("DoctorList");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteDoctor(int id)
        {
            string userName = User.Identity.Name;
            doctorBl.DeleteDoctor(id, userName);
            TempData["RedirectPartial"] = true;
            return RedirectToAction("DoctorList");
        }


        [HttpGet]
        [Authorize(Roles = "Doctor,Admin")]
        public ActionResult EditDoctorByName(string user)
        {
            SelectList list = new SelectList(
                 new List<SelectListItem>
                 {
                    new SelectListItem {Value="1", Text = "Male" },
                    new SelectListItem {Value="2", Text ="Female" }
                 }, "Value", "Text");
            ViewBag.Sex = list;
            Doctor doctor = doctorBl.GetDoctorByName(user);
            ViewBag.Type = new SelectValue().GetDoctorType();
            TempData["UrlPictue"] = doctor.Photo;
            return View(doctor);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public ActionResult EditDoctorByName(Doctor doctor)
        {
            string userName = User.Identity.Name;
            string url = (string)TempData["UrlPictue"];
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                if (file != null)
                {
                    var name = DateTime.Now.ToFileTime() + Path.GetFileName(file.FileName);
                    if (file.ContentLength > 0)
                    {
                        // лучше оставить слеши Content/
                        if (!Directory.Exists(Server.MapPath("~/Content/Images/DifferentPictures/")))
                        {   // лучше оставить слеши Content/
                            Directory.CreateDirectory(Server.MapPath("~/Content/Images/DifferentPictures/"));
                        }
                        // после Content можно не ставить / слэш
                        string path = Path.Combine(Server.MapPath("~/Content/Images/DifferentPictures") + "/" + name);
                        file.SaveAs(path);

                        if (url != doctor.Photo)
                        {
                            if (doctor.Sex == "1")
                            {
                                doctor.Sex = "Male";
                            }
                            else
                            {
                                doctor.Sex = "Female";
                            }
                            doctor.WhoModified = userName;
                            doctor.WhenModified = DateTime.Now;
                            doctor.Photo = "/Content/Images/DifferentPictures/" + name;
                            ViewBag.Type = new SelectValue().GetDoctorType();
                            db.Entry(doctor).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            if (doctor.Sex == "1")
                            {
                                doctor.Sex = "Male";
                            }
                            else
                            {
                                doctor.Sex = "Female";
                            }
                            doctor.WhoModified = userName;
                            doctor.WhenModified = DateTime.Now;
                            doctor.Photo = url;
                            db.Entry(doctor).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                if (doctor.Sex == "1")
                {
                    doctor.Sex = "Male";
                }
                else
                {
                    doctor.Sex = "Female";
                }
                doctor.WhoModified = userName;
                doctor.WhenModified = DateTime.Now;
                doctor.Photo = url;
                ViewBag.Type = new SelectValue().GetDoctorType();
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }

        #endregion
        
        #region Private doctor's page
        [Authorize(Roles ="Doctor")]
        public ActionResult DoctorPage(string doctorName)
        {
            Doctor user = doctorBl.GetDoctorByName(doctorName);
            return View(user);
        }
        #endregion

        #region Doctor Type 
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult TypeList(int?page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            List<DoctorType> list = doctorBl.ListDoctorType();
            return View(list.ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateType()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateType(DoctorType type)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                doctorBl.AddDoctorType(type, userName);
                return RedirectToAction("TypeList");
            }
            return View(type);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditType(int id)
        {
            DoctorType type = doctorBl.GetDoctorType(id);
            return View(type);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditType(DoctorType type)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                doctorBl.EditDoctorType(type, userName);
                return RedirectToAction("TypeList");
            }
            return View(type);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteType(int id)
        {
            string userName = User.Identity.Name;
            doctorBl.DeleteDoctorType(id, userName);
            TempData["RedirectPartial"] = true;
            return RedirectToAction("TypeList");
        }
        #endregion

        #region Appointmants

        public ActionResult _DoctorsAppointmants()
        {
            DateTime now = DateTime.Now;
            int day = now.Day-1;
            int year = now.Year;
            int month = now.Month;
            now = new DateTime(year, month, day);

            string user = User.Identity.Name;
            Doctor doctor = db.Doctors.Where(x => x.UserName == user).FirstOrDefault();
            List<Schedule> list = db.Schedule.Where(x => x.DoctorId == doctor.DoctorId && x.isBooked == true).ToList();
            var f = list.SkipWhile(x => x.Date < now).GroupBy(x => x.Date).Select(x => x.First()).Take(7).ToList();
            ViewBag.Date = f;
            ViewBag.Doctor = doctor.DoctorId;
            return PartialView(list);
        }

        public ActionResult AppointmantsDay(int id, DateTime date)
        {
            List<Schedule> list = db.Schedule.Where(x => x.DoctorId == id && x.Date == date && x.isBooked == true).ToList();
            return View(list);
        }
        #endregion

        #region Short information about doctor

        public ActionResult ShortDoctorInfo(int id)
        {
            Doctor doctor = doctorBl.GetDoctor(id);
            return View(doctor);
        }


        #endregion
    }
}