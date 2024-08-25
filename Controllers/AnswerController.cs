using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using BookReaders.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookReaders.Controllers
{
    public class AnswerController : Controller
    {
        private readonly AppDbContext _dbContext;


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AnswerController(AppDbContext dbContext,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create(Answer answer)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                answer.CreatedAt = DateTime.Now;
                answer.UserId = currentUser.Id;

                _dbContext.Answers.Add(answer);
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Question");

            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "AccountUser" });
            }
          

        }
        [HttpPost]

        public ActionResult Delete(int id, Answer answer)
        {
            var item = _dbContext.Answers.Where(x => x.Id == id).FirstOrDefault();

            if (item != null)
            {
                _dbContext.Answers.Remove(item);

                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Question" );
            }
            return View();
        }
    }
}
