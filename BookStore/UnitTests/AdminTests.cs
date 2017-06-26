using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using Domain.Entities;
using System.Collections.Generic;
using WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Books()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1" },
                new Book {BookId = 2, Name = "Book2" },
                new Book {BookId = 3, Name = "Book3" },
                new Book {BookId = 4, Name = "Book4" },
                new Book {BookId = 5, Name = "Book5" }
            });
            AdminController controlelr = new AdminController(mock.Object);
            List<Book> result = ((IEnumerable<Book>)controlelr.Index().ViewData.Model).ToList();

            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual(result[0].BookId, 1);
            Assert.AreEqual(result[1].BookId,2);
        }
        [TestMethod]
        public void Can_Edit_Book()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1" },
                new Book {BookId = 2, Name = "Book2" },
                new Book {BookId = 3, Name = "Book3" },
                new Book {BookId = 4, Name = "Book4" },
                new Book {BookId = 5, Name = "Book5" }
            });
            AdminController controlelr = new AdminController(mock.Object);

            Book book1 = (Book)controlelr.Edit(1).ViewData.Model;
            Book book2 = (Book)controlelr.Edit(2).ViewData.Model;
            Book book3 = (Book)controlelr.Edit(3).ViewData.Model;

            Assert.AreEqual(1, book1.BookId);
            Assert.AreEqual(2, book2.BookId);
            Assert.AreEqual(3, book3.BookId);
        }

        [TestMethod]
        public void Cannon_edit_Nonexistent_Book()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1" },
                new Book {BookId = 2, Name = "Book2" },
                new Book {BookId = 3, Name = "Book3" },
                new Book {BookId = 4, Name = "Book4" },
                new Book {BookId = 5, Name = "Book5" }
            });
            AdminController controlelr = new AdminController(mock.Object);

            Book result = (Book)controlelr.Edit(7).ViewData.Model;

            Assert.IsNull(result);
        }
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            AdminController controller = new AdminController(mock.Object);
            Book book = new Book { Name = "test" };

            ActionResult reuslt = controller.Edit(book);
            mock.Verify(m => m.SaveBook(book));
            Assert.IsNotInstanceOfType(reuslt, typeof(ViewResult));
        }
    }
}
