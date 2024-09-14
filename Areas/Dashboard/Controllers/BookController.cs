using BookReaders.Data;
using BookReaders.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Reflection.Metadata.BlobBuilder;


namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class BookController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchItem { get; set; }
        public BookController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;

            // Initialize 
            PageSize = 10;
            CurrentPage = 1;
            TotalPages = 0;
            SearchItem = "";

        }

        [HttpGet]
   
       public IActionResult Index(string SearchItem, int currentpage = 1, bool showLowQuantity = false)
            {


            int pageSize = 6;
            var books = _dbContext.Books.Include(A => A.Author).Include(C => C.Category).AsQueryable();

        /*    var lowq = 0 ;
            var bookname="";
            foreach (var book in books)
            {
                if (book.Quantity < 2)
                {
                    lowq++;
                    bookname += book.Title+ "<br/>";

                }
            }
            ViewBag.Warning = lowq+ bookname;*/

            if (showLowQuantity)  // Test with hardcoded filter
            {
                books = books.Include(x => x.Author).Include(C => C.Category).Where(book => book.Quantity < 2).AsQueryable();

            }
          

            else if (SearchItem == null)
            {
                books = _dbContext.Books.Include(A => A.Author).Include(C => C.Category).AsQueryable();
            }

            else
            {
                books = books.Include(x => x.Author).Include(C => C.Category).Where(x => x.Title.Contains(SearchItem)
                   || x.Description.Contains(SearchItem)
                   || x.Language.Contains(SearchItem)
                   || x.Category.Name.Contains(SearchItem)
                   || x.Author.Name.Contains(SearchItem)
                   || x.PublishDate.Year.ToString().Contains(SearchItem)).AsQueryable();

              
            }
            // Materialize the query before pagination
            var filteredBooks = books.ToList();

            int totalRecords = filteredBooks.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var paginatedBooks = filteredBooks.Skip((currentpage - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToList();

            ViewBag.CurrentPage = currentpage;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchItem = SearchItem;





            return View(paginatedBooks);
         

        }
  

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

      /*  public ActionResult Search(string SearchItem)
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
        }*/
    }
}
