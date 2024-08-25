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
           
            if (books.Any())
            {
                foreach (var book in books)
                {
                    if ((DateTime.Now - book.BorrowDate).TotalDays >= 7)
                    {
                      /*  SendMessage(book.User.Id, $" Dear {book.User.UserName} Please Return the Book {book.Book.Title} you delay {(DateTime.Now - book.BorrowDate).TotalDays}");*/
                        SendMessage(book.User.Id, "Dear " + book.User.UserName + ",\n\n" +
    "We kindly remind you to return the book '" + book.Book.Title + "' that you have exceeded the borrowing period for by " +
    (DateTime.Now - book.BorrowDate).TotalDays + " days.\n\n" +
    "Your prompt attention to this matter would be greatly appreciated. Returning the book promptly ensures that others can benefit from its availability.\n\n" +
    "Thank you for your cooperation and understanding.\n\n" +
    "Best regards,\n[Your Name/Organization Name]");
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

        public ActionResult Delete(int id, Borrow borrow)
        {
            var item = _dbContext.Borrows.Where(x => x.Id == id).FirstOrDefault();

            _dbContext.Borrows.Remove(item);
            _dbContext.SaveChanges();
            TempData["successData"] = "Book has been Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
