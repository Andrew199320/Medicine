using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        private List<CartLine> linearCollection = new List<CartLine>();

        public IEnumerable<CartLine> Lines { get { return linearCollection; } }

        public void AddItem(Book book, int quantity)
        {
            CartLine line = linearCollection.Where(b => b.Book.BookId == book.BookId).FirstOrDefault();
            if (line == null)
            {
                linearCollection.Add(new CartLine {Book = book, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Book book)
        {
            linearCollection.RemoveAll(b=>b.Book.BookId == book.BookId);
        }
        public decimal ComputeTotalValue()
        {
            return linearCollection.Sum(e => e.Book.Price * e.Quantity);
        }
        public void Clear()
        {
            linearCollection.Clear();
        }

    }
    public class CartLine
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
