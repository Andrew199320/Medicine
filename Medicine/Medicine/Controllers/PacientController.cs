using Medicine.Models;
using Medicine.Models.BusinessLogic;
using Medicine.Models.Pacients;
using Medicine.Models.Schedules;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medicine.Controllers
{
    public class PacientController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly PacientBL pacientBl = new PacientBL();

        #region Pacient
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult PacientList(int?page)
        {
            int pageSize = 15;
            int pageNumber = page ?? 1;
            List<Pacient> list = pacientBl.ListPacient();
            return View(list.ToPagedList(pageNumber,pageSize));
        }
        [Authorize(Roles = "User")]

        [HttpGet]
        public ActionResult CreatePacient(string email)
        {
            SelectList list = new SelectList(
                 new List<SelectListItem>
                 {
                    new SelectListItem {Value="1", Text = "Male" },
                    new SelectListItem {Value="2", Text ="Female" }
                 }, "Value", "Text");
            ViewBag.Sex = list;
            return View();
        }
        [Authorize(Roles = "User")]

        [HttpPost]
        public ActionResult CreatePacient(Pacient pacient)
        {

            string userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                try
                {
                    pacientBl.AddPacient(pacient, userName);
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
                                pacient.Picture = "/Content/Images/DifferentPictures/" + name;
                                db.Entry(pacient).State = EntityState.Modified;
                                db.SaveChanges();

                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return View(pacient);
                }
            }

            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "User")]

        [HttpGet]
        public ActionResult EditPacient(string user)
        {
            SelectList list = new SelectList(
              new List<SelectListItem>
              {
                    new SelectListItem {Value="1", Text = "Male" },
                    new SelectListItem {Value="2", Text ="Female" }
              }, "Value", "Text");
            ViewBag.Sex = list;
            Pacient pacient = pacientBl.GetPacient(user);
            TempData["UrlPictue"] = pacient.Picture;

            return View(pacient);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult EditPacient(Pacient pacient)
        {
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

                        if (url != pacient.Picture)
                        {
                            if (pacient.Sex == "1")
                            {
                                pacient.Sex = "Male";
                            }
                            else
                            {
                                pacient.Sex = "Female";
                            }
                            pacient.WhoModified = User.Identity.Name;
                            pacient.WhenModified = DateTime.Now;
                            pacient.Picture = "/Content/Images/DifferentPictures/" + name;
                            db.Entry(pacient).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            if (pacient.Sex == "1")
                            {
                                pacient.Sex = "Male";
                            }
                            else
                            {
                                pacient.Sex = "Female";
                            }
                            pacient.WhoModified = User.Identity.Name;
                            pacient.WhenModified = DateTime.Now;
                            pacient.Picture = url;
                            db.Entry(pacient).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                if (pacient.Sex == "1")
                {
                    pacient.Sex = "Male";
                }
                else
                {
                    pacient.Sex = "Female";
                }
                pacient.WhoModified = User.Identity.Name;
                pacient.WhenModified = DateTime.Now;
                pacient.Picture = url;
                db.Entry(pacient).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeletePacient(int id)
        {
            string userName = User.Identity.Name;
            pacientBl.DeletePacient(id, userName);
            TempData["RedirectPartial"] = true;
            return RedirectToAction("PacientList");
        }
        #endregion

        #region DiseasHistory

        public ActionResult GetUserById(int id)
        {
            List<DiseaseHistory> list = db.DiseaseHistories.Where(x => x.PacientId == id && x.IsVisible).OrderByDescending(x=>x.DiseaseHistoryId).ToList();
            Pacient pacient = db.Pacients.Where(x => x.PacientId == id).FirstOrDefault();
            ViewBag.Pacient = pacient;
            return View(list);
        }
        public ActionResult _DiseaseHistoryPartial()
        {
            string user = User.Identity.Name;
            Pacient pacient = new PacientBL().GetPacientByName(user);
            List<DiseaseHistory> history = db.DiseaseHistories.Where(x => x.PacientId == pacient.PacientId).ToList();
            return PartialView(history);
        }

        [HttpPost]
        public ActionResult CreateDeasisHistory( string disName, DateTime startFrom, string contraindications, string additionalInformation, int id)
        {
            DiseaseHistory history = new DiseaseHistory();
            history.DiseaseName = disName;
            history.StartFrom = startFrom;
            history.Contraindications = contraindications;
            history.AdditionalInformation = additionalInformation;
            history.IsVisible = true;
            history.WhoAdded = User.Identity.Name;
            history.WhenAdded = DateTime.Now;
            history.PacientId = id;
            db.DiseaseHistories.Add(history);
            db.SaveChanges();

            return RedirectToAction("GetUserById", new { id = id });
        }
        #endregion

        #region PacientView
        [Authorize(Roles = "User,Doctor")]
        public ActionResult PacientPage(string pacientName)
        {
            Pacient pacient = db.Pacients.Where(x => x.UserName == pacientName).FirstOrDefault();
            if (pacient == null)
            {
                return RedirectToAction("CreatePacient", "Pacient",new {email= pacientName});
            }
            else
            {
                Pacient user = pacientBl.GetPacientByName(pacientName);
                return View(user);
            }
           
        }

        #endregion

        #region PacientAppointmants

        [Authorize(Roles = "User")]
        public ActionResult _Appointmants()
        {
            DateTime now = DateTime.Now;
            int day = now.Day - 1;
            int year = now.Year;
            int month = now.Month;
            now = new DateTime(year, month, day);

            string u = User.Identity.GetUserName();
            Pacient p = db.Pacients.Where(x => x.UserName == u).FirstOrDefault();
            List<Schedule> s = db.Schedule.Where(x => x.PacientId == p.PacientId && x.isBooked == true).ToList();
            s = s.SkipWhile(x => x.Date < now).ToList();
            return PartialView(s);
        }

        #endregion

        

    }
}