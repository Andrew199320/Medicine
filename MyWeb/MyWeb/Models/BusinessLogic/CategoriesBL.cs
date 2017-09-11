using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyWeb.Models.BusinessLogic
{
    public class CategoriesBL
    {
        private readonly ApplicationDbContext db;
        public CategoriesBL()
        {
            db = new ApplicationDbContext();
        }

        public List<Categories> ListCategories()
        {
            List<Categories> list = db.Categories.Where(x => x.IsVisible).OrderByDescending(x=>x.WhenAdded).ToList();
            return list;
        }

        public Categories GetCategoryById(int id)
        {
            Categories category = db.Categories.Where(x => x.IdCategory == id && x.IsVisible == true).FirstOrDefault();
            return category;
        }

        public Categories AddCategory(Categories category, string userName)
        {
            category.WhoAdded = userName;
            category.WhenAdded = DateTime.Now;
            category.IsVisible = true;
            db.Categories.Add(category);
            db.SaveChanges();
            return category;
        }
        public Categories ChangeCategory(Categories category/*, string userName*/)
        {
            //category.WhoModified = userName;
            //category.WhenModified = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return category;
        }
        public void Delete(int id, string userName)
        {
            Categories category = db.Categories.Where(x => x.IdCategory == id && x.IsVisible == true).FirstOrDefault();
            category.IsVisible = false;
            category.WhoDeleted = userName;
            category.WhenDeleted = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}