using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using BookReaders.Models;
using BookReaders.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookReaders.Controllers
{

    public class BookController : Controller
    {
        private readonly AppDbContext _dbContext;

        private IRepository<Book> _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public BookController(AppDbContext dbContext, IRepository<Book> repository,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _repository = repository;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }

        /*   public IActionResult AutoComplete(string search)
           {
               var books = _dbContext.Books
                   .Where(book => book.Title.Contains(search))
                   .Select(book => new { Title = book.Title, Id = book.Id })
                   .ToList();

               return Json(books);
           }*/
        public JsonResult AutoComplete(string term)
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
            return View(books);
        }




        }

    }

