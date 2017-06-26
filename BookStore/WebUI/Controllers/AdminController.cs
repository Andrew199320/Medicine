using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IBookRepository repository;
        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index()
        {
            return View(repository.Books);
        }
        public ViewResult Edit(int bookId)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookId == bookId);
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                repository.SaveBook(book);
                TempData["message"] = string.Format("Book's chnages \"{0} saved", book.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(book);
            }
            
        }
    }
}