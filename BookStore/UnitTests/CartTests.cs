using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using Domain.Abstract;
using Moq;
using WebUI.Controllers;
using System.Web.Mvc;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Cart_Add_New_Lines()
        {
            // Organisation 
            Book book1 = new Book { BookId = 1, Name = "First book" };
            Book book2 = new Book { BookId = 2, Name = "Second book" };

            //Action 
            Cart cart = new Cart();
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            List<CartLine> result = cart.Lines.ToList();

            // Statement
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0].Book, book1);
            Assert.AreEqual(result[1].Book, book2);
        }
        [TestMethod]
        public void Cart_Add_Additional_Line_For_Existing_Line()
        {
            // Organisation 
            Book book1 = new Book { BookId = 1, Name = "First book" };
            Book book2 = new Book { BookId = 2, Name = "Second book" };

            //Action 
            Cart cart = new Cart();
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            cart.AddItem(book1, 5);
            List<CartLine> result = cart.Lines.OrderBy(x=>x.Book.BookId).ToList();

            // Statement
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0].Quantity,6);
            Assert.AreEqual(result[1].Quantity,1);

        }
        [TestMethod]
        public void Cart_Delete_Books()
        {
            // Organisation 
            Book book1 = new Book { BookId = 1, Name = "First book" };
            Book book2 = new Book { BookId = 2, Name = "Second book" };
            Book book3 = new Book { BookId = 3, Name = "Third book" };

            //Action 
            Cart cart = new Cart();
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            cart.AddItem(book1, 5);
            cart.AddItem(book3, 2);
            cart.RemoveLine(book2);


            // Statement
            Assert.AreEqual(cart.Lines.Where(x => x.Book == book2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);

        }
        [TestMethod]
        public void Cart_Calculate_Total_Cost()
        {
            // Organisation 
            Book book1 = new Book { BookId = 1, Name = "First book", Price = 100 };
            Book book2 = new Book { BookId = 2, Name = "Second book", Price = 200};

            //Action 
            Cart cart = new Cart();
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            cart.AddItem(book1, 5);
            decimal result = cart.ComputeTotalValue();

            // Statement
            Assert.AreEqual(result, 800);
        }
        [TestMethod]
        public void Cart_Clear_Cart()
        {
            // Organisation 
            Book book1 = new Book { BookId = 1, Name = "First book", Price = 100 };
            Book book2 = new Book { BookId = 2, Name = "Second book", Price = 200 };

            //Action 
            Cart cart = new Cart();
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            cart.AddItem(book1, 5);
            cart.Clear();

            // Statement
            Assert.AreEqual(cart.Lines.Count(), 0);
        }
        // Adding element to the cart
        [TestMethod]
        public void Cart_Add_To_Cart()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book> {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1" },

            }.AsQueryable());

            Cart cart = new Cart();
            CartController controller = new CartController(null,null);
            controller.AddToCart(cart, 1, null);
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Book.BookId, 1);
        }
        // redicretion to the Index page after book was added
        [TestMethod]
        public void Cart_Add_To_Cart_Redirect_To_Start_Screen()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(x => x.Books).Returns(new List<Book> {
                new Book {BookId = 1, Name = "Book1", Genre = "Genre1" },

            }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(null,null);

            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();
            CartController target = new CartController(null,null);
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shipingDetails = new ShippingDetails();

            CartController controller = new CartController(null,mock.Object);
            ViewResult result = controller.Checkout(cart, shipingDetails);

            mock.Verify(x => x.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            CartController controller = new CartController(null, mock.Object);
            controller.ModelState.AddModelError("Error", "Error");

            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            mock.Verify(x => x.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Cannot_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            CartController controller = new CartController(null, mock.Object);

            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            mock.Verify(x => x.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }


    }
}
