using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Controllers
{
    public class MessageController : Controller
    {
        private readonly AppDbContext _dbContext;


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public MessageController(AppDbContext dbContext,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser != null)
                {
                    var message = await _dbContext.Messages.Include(a => a.User).FirstOrDefaultAsync(x => x.UserId == currentUser.Id);

                    if (message != null)
                    {
                        return View(message);
                    }
                }
            }

            // If user is not authenticated or no message is found, return a default view
            return View();
        }
    }
}
