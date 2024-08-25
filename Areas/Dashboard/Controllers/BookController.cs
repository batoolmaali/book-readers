using BookReaders.Data;
using BookReaders.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class BookController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private int pageItem;
        public BookController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            pageItem = 5;
        }

        [HttpGet]
        public IActionResult Index(int? id)
        {


            var Books = _dbContext.Books.Include(A => A.Author).Include(C => C.Category).ToList();
                return View(Books);
         

        }
        /* public IActionResult Index(int? id)
         {
             if (id == 0 || id == null)
             {
                 var Books = _dbContext.Books.Include(A => A.Author).Include(C => C.Category).ToList().Take(pageItem);
                 return View(Books);
             }

             else 
             {
                 var Books = _dbContext.Books.Include(A => A.Author).Include(C => C.Category).Where(x => x.Id > id).ToList().Take(pageItem);
                 return View(Books);

             }

         }*/

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Cat = _dbContext.Categories.ToList();
            ViewBag.Authors = _dbContext.Authors.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book , IFormFile Image, IFormFile FilePDF)
        {
            if (Image != null)
            {
                string folder = "Images/BookImages";
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }


                book.ImagePath = Path.Combine(folder, fileName);
            }
                if (FilePDF != null)
                {
                    string folder1 = "Images/BookPDF";
                    string fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(FilePDF.FileName);
                    string filePath1 = Path.Combine(_webHostEnvironment.WebRootPath, folder1, fileName1);

                    using (var stream = new FileStream(filePath1, FileMode.Create))
                    {
                        FilePDF.CopyTo(stream);
                    }


                    book.BookPDF = Path.Combine(folder1, fileName1);

                    _dbContext.Books.Add(book);
                _dbContext.SaveChanges();
                TempData["successData"] = "Book has been Created successfully";
                
                return RedirectToAction("Index");
            }

            return View();
        }

     


        [HttpGet]

        public ActionResult EditBook(int id)
        {
           
          
            var item = _dbContext.Books.Where(x => x.Id == id).FirstOrDefault();
            if (item != null)
            {
                ViewBag.Cat = _dbContext.Categories.ToList();

                ViewBag.Authors = _dbContext.Authors.ToList(); 
                return View(item);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditBook(int id, Book book, IFormFile Image)
        {
            try
            {
                var item = _dbContext.Books.FirstOrDefault(x => x.Id == id);

                if (item != null && Image != null)
                {
                    string folder = "Images/BookImages";
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Image.CopyTo(stream);
                    }

                    item.Title = book.Title;
                    item.Author = book.Author;
                    item.Description = book.Description;
                    item.Quantity = book.Quantity;
                    item.IsAvailable = book.IsAvailable;
                    item.AuthorId = book.AuthorId;
                    item.CategoryId = book.CategoryId;
                    item.Language = book.Language;
                    item.PublishDate = book.PublishDate;
                    item.ImagePath = Path.Combine(folder, fileName);

                    _dbContext.SaveChanges();
                    TempData["successData"] = "Book has been updated successfully";
                    return RedirectToAction("Index");
                    
                }

                return View();
            }
            catch (Exception ex)
            {
                
                return View("Error");
            }
        }
        [HttpPost]

        public ActionResult Delete(int id , Book book)
        {
            var item=_dbContext.Books.Where(x=>x.Id == id).FirstOrDefault();
            _dbContext.Books.Remove(item);
            _dbContext.SaveChanges();
            TempData["successData"] = "Book has been Deleted successfully";
            return RedirectToAction("Index");
        }

        public ActionResult Search(string SearchItem)
        {

            var books = _dbContext.Books.AsQueryable();

            if (SearchItem == null)
            {
                books = _dbContext.Books.AsQueryable();
            }

            else
            {
                books = books.Include(x=>x.Author).Include(C => C.Category).Where(x => x.Title.Contains(SearchItem)
                   || x.Description.Contains(SearchItem)
                   ||x.Language.Contains(SearchItem)
                   || x.Category.Name.Contains(SearchItem)
                   || x.Author.Name.Contains(SearchItem)
                   || x.PublishDate.Year.ToString().Contains(SearchItem)).AsQueryable();

                return View("Index", books);
            }

            return View("Index", books);
        }
    }
}
