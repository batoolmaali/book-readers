using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace BookReaders.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AppDbContext _dbContext;


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Term { get; set; }

        public AuthorController(AppDbContext dbContext,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            // Initialize 
            PageSize = 10;
            CurrentPage = 1;
            TotalPages = 0;
            Term = "";


        }
       
    /*    public IActionResult Index()
        {
            var author=_dbContext.Authors.Include(b=>b.Books).ToList();
            return View(author);
        }

*/



        public IActionResult Index(string term, int currentpage = 1)
        {
            int pageSize = 8;

            var query = _dbContext.Authors.Include(b => b.Books).AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                var AutherName = query.Where(a => a.Name.Contains(term)).Select(a => a.Name).ToList();
                return Json(AutherName);
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




        public IActionResult AuthorDetails(int id)
        {

            var author=_dbContext.Authors.Include(x => x.Books).Where(x=>x.Id==id).FirstOrDefault();

            var books= _dbContext.Books.Where(a=>a.AuthorId==id).ToList();

            var BestBook = _dbContext.Borrows.Where(b => b.Book.AuthorId == id)
                .GroupBy(u => u.Book.Title)
                            .Select(group => new
                            {
                                book = group.Key,
                                Count = group.Count()
                            }).OrderByDescending(x => x.Count)
                             .FirstOrDefault();

            if(BestBook != null)
            {
                ViewBag.BestBook = BestBook;
            }
           


           return View(author);
        }
    }
}
