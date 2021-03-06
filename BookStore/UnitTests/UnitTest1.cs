﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using System.Collections.Generic;
using Domain.Entities;
using WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.HtmlHelpers;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Cat_Paginate()
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
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            BooksListViewModel result = (BooksListViewModel)controller.List(null, 2).Model;

            List<Book> books = result.Books.ToList();
            Assert.IsTrue(books.Count == 2);
            Assert.AreEqual(books[0].Name, "Book4");
            Assert.AreEqual(books[1].Name, "Book5");
        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;
            PaginInfo pagingInfo = new PaginInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10               
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }
        [TestMethod]
        public void Can_Send_PaginationViewModel()
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
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            BooksListViewModel result = (BooksListViewModel)controller.List(null,2).Model;
            PaginInfo pagInfo = result.PagingInfo;
            Assert.AreEqual(pagInfo.CurrentPage, 2);
            Assert.AreEqual(pagInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagInfo.TotalItems, 5);
            Assert.AreEqual(pagInfo.TotalPages, 2);
        }
        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1" },
                new Book {BookId = 2, Name = "Book2", Genre = "Genre2" },
                new Book {BookId = 3, Name = "Book3", Genre = "Genre2" },
                new Book {BookId = 4, Name = "Book4", Genre = "Genre1" },
                new Book {BookId = 5, Name = "Book5", Genre = "Genre2" }
            });
            NavController target = new NavController(mock.Object);

            List<string> result = ((IEnumerable<string>)target.Menu().Model).ToList();

            BooksController controller = new BooksController(mock.Object);
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0], "Genre1");
            Assert.AreEqual(result[1], "Genre2");
        }
  
        [TestMethod]
        public void Indicates_Selected_Genre()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1" },
                new Book {BookId = 2, Name = "Book2", Genre = "Genre2" },
                new Book {BookId = 3, Name = "Book3", Genre = "Genre2" },
                new Book {BookId = 4, Name = "Book4", Genre = "Genre1" },
                new Book {BookId = 5, Name = "Book5", Genre = "Genre2" }
            });
            NavController target = new NavController(mock.Object);

            string genreSelected = "Genre2";
            string result = target.Menu(genreSelected).ViewBag.SelectedGenre;
            Assert.AreEqual(genreSelected, result);
        }
        [TestMethod]
        public void Generate_Genre_Specific_Book_Count()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book>
            {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1" },
                new Book {BookId = 2, Name = "Book2", Genre = "Genre2" },
                new Book {BookId = 3, Name = "Book3", Genre = "Genre2" },
                new Book {BookId = 4, Name = "Book4", Genre = "Genre1" },
                new Book {BookId = 5, Name = "Book5", Genre = "Genre2" }
            });
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 2;

            int res1 = ((BooksListViewModel)controller.List("Genre1").Model).PagingInfo.TotalItems;
            int res2 = ((BooksListViewModel)controller.List("Genre2").Model).PagingInfo.TotalItems;
            int resAll = ((BooksListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 3);
            Assert.AreEqual(resAll, 5);
        }
    }
}
