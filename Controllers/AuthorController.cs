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
        public AuthorController(AppDbContext dbContext,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }
       
        public IActionResult Index()
        {
            var author=_dbContext.Authors.Include(b=>b.Books).ToList();
            return View(author);
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
            ViewBag.BestBook = BestBook;


           return View(author);
        }
    }
}
