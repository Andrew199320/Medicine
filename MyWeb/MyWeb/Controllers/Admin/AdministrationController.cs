using MyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Controllers.Admin
{
    public class AdministrationController : Controller
    {
        private ApplicationDbContext db;
        public AdministrationController()
        {
            db = new ApplicationDbContext();
        }
       

        public ActionResult AdminPanel()
        {
          
            return View();
        }
    }
}