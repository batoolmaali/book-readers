using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using BookReaders.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookReaders.Controllers
{
    public class QuoteController : Controller
    {
        private readonly AppDbContext _dbContext;


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
		public int PageSize { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public string Term { get; set; }

		public QuoteController(AppDbContext dbContext,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
         


        }
        public IActionResult Index(int currentpage = 1)
        {
			int pageSize = 3;
			var Quotes=_dbContext.Quotes.Include(u=>u.User).AsQueryable();
		
			var filteredQuotes = Quotes.ToList();

			int totalRecords = filteredQuotes.Count;
			int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

			var paginatedQuotes = filteredQuotes.Skip((currentpage - 1) * pageSize)
											  .Take(pageSize)
											  .ToList();

			ViewBag.CurrentPage = currentpage;
			ViewBag.TotalPages = totalPages;
			ViewBag.PageSize = pageSize;
			
			return View(paginatedQuotes);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Quote quote)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var qu = new Quote()
                {
                    QuoteText = quote.QuoteText,
                    BookName = quote.BookName,
                    UserId = currentUser.Id
                };
             


                    _dbContext.Quotes.Add(qu);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index");
             
             
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "AccountUser" });
            }
        }

		[HttpPost]

		public ActionResult Delete(int id)
		{
			var item = _dbContext.Quotes.Where(x => x.Id == id).FirstOrDefault();

			if (item != null)
			{
				_dbContext.Quotes.Remove(item);

				_dbContext.SaveChanges();

				return RedirectToAction("Index");
			}
			return View();
		}

	}
}
