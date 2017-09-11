using MyWeb.Models;
using MyWeb.Models.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Controllers.CategoriesC
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly CategoriesBL bl;

        public CategoryController()
        {
            db = new ApplicationDbContext();
            bl = new CategoriesBL();
        }

        public ActionResult Categories()
        {
           
            return View();
        }
        public ActionResult GetCategories()
        {
            List<Categories> list = bl.ListCategories();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCategoryById(int id)
        {
            Categories category = bl.GetCategoryById(id);
            return Json(category,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateCategory([Bind(Exclude = "IdCategory")] Categories category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                //Categories newCat = bl.AddCategory(category,userName);
                return RedirectToAction("Categories");
            }
            return Json(category, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditCategory(Categories category)
        {
            if (ModelState.IsValid)
            {
                //Categories newCat = bl.AddCategory(category);
                return RedirectToAction("Categories");
            }
            return Json(category, JsonRequestBehavior.AllowGet);
        }

       
    }
}