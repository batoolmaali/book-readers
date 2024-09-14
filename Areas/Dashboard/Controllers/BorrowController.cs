using BookReaders.Data;
using BookReaders.Models;
using BookReaders.Repository.Base;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class BorrowController : Controller
    {
        private readonly AppDbContext _dbContext;
        private IRepository<Borrow> _repository;

        public BorrowController(AppDbContext dbContext,
            IRepository<Borrow> repository)
        {
            _dbContext = dbContext;

            _repository = repository;
        }
        public IActionResult Index()
        {
            var books = _dbContext.Borrows.Include(x => x.Book).Include(y => y.User).ToList();

          /*  var xx = _dbContext.Borrows.Select(x =>
                           new ViewModelObject { 
                               dayleft = x.ReturnDate.Subtract(DateTime.Now).Days 
                           });*/

          


           /* var diff = books.FirstOrDefault().ReturnDate.Subtract(DateTime.Now);*/


            if (books.Any())
            {
                foreach (var book in books)
                {
                    DateTime date3 = book.ReturnDate.Date;
                    DateTime date4 = DateTime.Now.Date;

                    TimeSpan timeDifference1 = date3 - date4;
                    int daysDifference1 = timeDifference1.Days;
                    
                    if (book.IsReturned==false &&  daysDifference1 < 0)
                    {

                        /*  SendMessage(book.User.Id, $" Dear {book.User.UserName} Please Return the Book {book.Book.Title} you delay {(DateTime.Now - book.BorrowDate).TotalDays}");*/
                        SendMessage(book.User.Id, "Dear <strong>" + book.User.UserName + "</strong>,<br><br>" +
                "We kindly remind you to return the book'<font color='red'>" + book.Book.Title + "</font>' that you have exceeded the borrowing period for by " +
                daysDifference1 + " days.\n\n" +
                "Your prompt attention to this matter would be greatly appreciated. Returning the book promptly ensures that others can benefit from its availability.\n\n" +
                "Thank you for your cooperation and understanding.\n\n" +
                "Best regards,\n[BookReaders Library]");
                    }
                }

            }

            return View(books);
        }


        public IActionResult SendMessage(string id, string content)
        {
            // Check if a message with the same UserId already exists
            var existingMessage = _dbContext.Messages.FirstOrDefault(m => m.UserId == id);

            if (existingMessage != null)
            {
                // Update the existing message
                existingMessage.Content = content;
                existingMessage.CreateDate = DateTime.Now;
            }
            else
            {
                // Create a new message
                var newMessage = new Message()
                {
                    Content = content,
                    UserId = id,
                    CreateDate = DateTime.Now
                };
                _dbContext.Messages.Add(newMessage);
                _dbContext.SaveChanges();
            }



            return RedirectToAction("Index");
        }

        [HttpPost]

        public ActionResult Delete(int borroweId, int bookId, string userId, Borrow borrow)
        {
            var book = _dbContext.Books.FirstOrDefault(x => x.Id == bookId);

            if (book == null)
            {
                TempData["errorData"] = "Book not found";
                return RedirectToAction("Index");
            }

            book.Quantity++;

            var msg = _dbContext.Messages.FirstOrDefault(x => x.UserId == userId);
            if (msg != null)
            {
                _dbContext.Messages.Remove(msg);
            }
            if (borroweId != null)
            {
                var borrowe = _dbContext.Borrows.Where(x => x.BookId == bookId && x.UserId ==userId).FirstOrDefault();

                if (borrowe != null)
                {
                    borrow.IsReturned = true;
                    _dbContext.Borrows.Remove(borrowe);
                }
            }
            _dbContext.SaveChanges();

            TempData["successData"] = "Book has been deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
