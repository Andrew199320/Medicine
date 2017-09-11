using Art.Models;
using Art.Models.BusinessLogic;
using Art.Models.MainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Art.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly CategoryBl _bl;
        public CategoryController()
        {
            _db = new ApplicationDbContext();
            _bl = new CategoryBl();

        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetCategories()
        {
            List<Category> list = _bl.GetAllGategories();
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCategoryById(int id)
        {
            Category category = _bl.GetCategoryWithId(id);

            return Json(category, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateCategory([Bind(Exclude = "IdCategory")] Category category)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                _bl.AddCategory(category, userName);
            }
            return Json(category, JsonRequestBehavior.AllowGet);
        }

    }
}