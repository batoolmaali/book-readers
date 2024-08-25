using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using BookReaders.Models;
using BookReaders.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly AppDbContext _dbContext;

        private IRepository<Book> _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public FavoriteController(AppDbContext dbContext, IRepository<Book> repository,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _repository = repository;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> favorite()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUsername = await _userManager.GetUserAsync(User);

                var user = _dbContext.Users.Include(x => x.Favorites).ThenInclude(uf => uf.Book).FirstOrDefault(x => x.Id == currentUsername.Id);

                if (user != null)
                {
                    var favViewModel = new UserFavViewModel
                    {
                        
                        User = user,
                     
                        Favorites = user.Favorites
                    };

                    return View(favViewModel);
                }
            }

            // Handle the case where the user is not found or not authenticated
            return RedirectToAction("Index", "Home");
        }
      
        [HttpPost]
        public async Task<IActionResult> AddToFav(int id, Favorite favorite)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUsername = await _userManager.GetUserAsync(User);

                var book=_dbContext.Favorites.Where(x=>x.BookId== id).FirstOrDefault();

                if (book != null)
              
                {
                    ViewBag.message = "Already exzist";
                    return RedirectToAction("Index", "Home");
                }
                else {
                if (currentUsername != null)
                {
                   
                    var fav = new Favorite
                    {
                        UserId = currentUsername.Id,
                        BookId = id
                    };

                    _dbContext.Favorites.Add(fav);
                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction("favorite");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "AccountUser" });
            }
        }

        [HttpPost]

        public ActionResult Delete(int id, Favorite favorite)
        {
            var item = _dbContext.Favorites.Where(f => f.BookId == id).FirstOrDefault();
            _dbContext.Remove(item);
            _dbContext.SaveChanges();
            return RedirectToAction("favorite");
        }

    }
}
