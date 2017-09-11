using Art.Models.MainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art.Models.BusinessLogic
{
    public class CategoryBl
    {
        private readonly ApplicationDbContext db;
        public CategoryBl()
        {
            db = new ApplicationDbContext();
        }
        public List<Category> GetAllGategories()
        {
            List<Category> list = db.Categories.Where(x => x.IsVisible).ToList();
            return list;
        }
        public Category GetCategoryWithId(int id)
        {
            Category category = db.Categories.Where(x => x.IsVisible && x.IdCategory == id).FirstOrDefault();
            return category;
        }
        public Category AddCategory(Category category, string userName)
        {
            category.WhoAdded = userName;
            category.WhenAdded = DateTime.Now;
            category.IsVisible = true;
            db.Categories.Add(category);
            db.SaveChanges();
            return category;
        }
    }
}