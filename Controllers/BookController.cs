using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using BookReaders.Models;
using BookReaders.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookReaders.Controllers
{

    public class BookController : Controller
    {
        private readonly AppDbContext _dbContext;

        private IRepository<Book> _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Term { get; set; }
        public BookController(AppDbContext dbContext, IRepository<Book> repository,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _repository = repository;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

            // Initialize 
            PageSize = 10; 
            CurrentPage = 1; 
            TotalPages = 0; 
            Term = ""; 


        }

        /*   public IActionResult AutoComplete(string search)
           {
              var books = _dbContext.Books
                   .Where(book => book.Title.Contains(search))
                   .Select(book => new { Title = book.Title, Id = book.Id })
                   .ToList();

               return Json(books);
           }*/
        /*     public JsonResult AutoComplete(string term)
             {
                 var books = _dbContext.Books;
                 var bookTitles = books.Select(b => b.Title).Where(book => book.Contains(term)).ToList();
                 return Json(bookTitles);

             }

             public IActionResult Index()
             {
                 var Book=_dbContext.Books.Include(c => c.Category).Include(a=>a.Author).ToList();
                 return View(Book);
             }
     */
        /*   public IActionResult Index(string term)
           {
               if (!string.IsNullOrEmpty(term))
               {
                   var bookTitles = _dbContext.Books.Select(b => b.Title).Where(book => book.Contains(term)).ToList();
                   return Json(bookTitles);
               }





               var books = _dbContext.Books.Include(c => c.Category).Include(a => a.Author).ToList();
               return View(books);


           }*/

        public IActionResult Index(string term, int currentpage = 1)
        {
            int pageSize = 10;

            var query = _dbContext.Books.Include(c => c.Category).Include(a => a.Author).AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                var bookTitles = query.Where(b => b.Title.Contains(term)).Select(b => b.Title).ToList();
                return Json(bookTitles);
            }


            // Materialize the query before pagination
            var filteredBooks = query.ToList();

            int totalRecords = filteredBooks.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var paginatedBooks = filteredBooks.Skip((currentpage - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToList();

            ViewBag.CurrentPage = currentpage;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.Term = term;

            return View(paginatedBooks);
        }



        public IActionResult BookDetails(int id)
        {
           
            var book = _dbContext.Books.Include(x=>x.Category).Include(x=>x.Author).
                Include(b => b.Reviews).ThenInclude(u=>u.User).Where(x=>x.Id== id).FirstOrDefault();

            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookDetailsViewModel
            {
                NewReviewComment = "",
                Book = book,
                Reviews = book.Reviews != null ? book.Reviews.ToList() : new List<Review>()
            };

            return View("~/Views/Book/BookDetails.cshtml", viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddReview(BookDetailsViewModel viewModel)
        {
           
                if (User.Identity.IsAuthenticated)
                {
                    var currentUsername = await _userManager.GetUserAsync(User);
                    if (currentUsername != null)
                    {
                        var newReview = new Review
                        {
                            Comment = viewModel.NewReviewComment,
                            BookId = viewModel.Book.Id,
                            ReviewDate = DateTime.Now,
                            UserId = currentUsername.Id
                        };

                        _dbContext.Reviews.Add(newReview);
                        await _dbContext.SaveChangesAsync();

                    ViewBag.UserName = currentUsername.UserName; 

                    return RedirectToAction("BookDetails", "Book", new { area = "", id = viewModel.Book.Id  });
                    }
                    else
                    {
                    // Handle the case where the current username is not found
                    return RedirectToAction("Login", "Account"); // Redirect to the home page or another appropriate action
                }
                }
                else
                {
                    // Handle the case where the user is not authenticated
                    // You may want to redirect to the login page or display an error message
                    return RedirectToAction("Login", "Account");
                }
            }

          public IActionResult CatBooks(int id)
        {

            var books=_dbContext.Books.Include(a=>a.Author).Where(b=>b.CategoryId==id).ToList();

            ViewBag.cat=_dbContext.Categories.Where(c=>c.Id==id).Select(c=>c.Name).FirstOrDefault();

            return View(books);
        }

        public IActionResult ArabicBooks()
        {
            var books=_dbContext.Books.Where(x=>x.Language =="Arabic").Include(a=>a.Author).Include(c=>c.Category).ToList();
            return View(books);
        }

        }

    }
