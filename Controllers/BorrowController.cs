using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using BookReaders.Models;
using BookReaders.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Controllers
{
    public class BorrowController : Controller
    {
        private readonly AppDbContext _dbContext;

        private IRepository<Borrow> _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public BorrowController(AppDbContext dbContext, IRepository<Borrow> repository,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _repository = repository;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }
        public async Task<IActionResult> BorrowBook()
        {

            if (User.Identity.IsAuthenticated)
            {
                var currentUsername = await _userManager.GetUserAsync(User);

                var user = _dbContext.Users.Include(x => x.Borrows).ThenInclude(u => u.Book).ThenInclude(c=>c.Category).FirstOrDefault(x => x.Id == currentUsername.Id);

                if (user != null)
                {
                    var BorrowViewModel = new UserBorrowViewModel
                    {
                        User = user,
                        Borrows = user.Borrows
                    };

                    return View(BorrowViewModel);
                }
            }


            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AddToBorrow(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUsername = await _userManager.GetUserAsync(User);
                if (currentUsername != null)
                {
                    var borrow1 = _dbContext.Borrows.Where(x => x.UserId == currentUsername.Id).Where(x => x.BookId == id).FirstOrDefault();

                    if (borrow1 != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }


                    var book = _dbContext.Books.FirstOrDefault(x => x.Id == id);

                    if (book != null && book.Quantity > 0)
                    {
                        book.Quantity--;
                        /*_dbContext.SaveChanges();*/

                        var borrow = new Borrow
                        {
                            BookId = id,
                            UserId = currentUsername.Id,
                            BorrowDate = DateTime.Now,
                            ReturnDate = DateTime.Now.AddDays(7),
                            IsReturned = false
                        };

                        _dbContext.Borrows.Add(borrow);
                        await _dbContext.SaveChangesAsync();


                        return RedirectToAction("BorrowBook");
                    }

                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "AccountUser" });
            }


            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Return(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUsername = await _userManager.GetUserAsync(User);
                if (currentUsername != null)
                {
                    var book = _dbContext.Books.FirstOrDefault(x => x.Id == id);
                    book.Quantity++;


                    var msg = _dbContext.Messages.FirstOrDefault(x => x.UserId == currentUsername.Id);
                    if (msg != null)
                    {
                        _dbContext.Remove(msg);
                    }

                    var borrow = _dbContext.Borrows.FirstOrDefault(x => x.BookId == id && x.UserId == currentUsername.Id);

                    borrow.IsReturned = true;




                    _dbContext.SaveChanges();
                    return RedirectToAction("BorrowBook");
                }
            }
            else
            {

            }
            return View();
        }


        [Authorize]
        public async Task<IActionResult> BorrowConfirmation(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUsername = await _userManager.GetUserAsync(User);
                var userId = currentUsername.Id;

                var borrows = _dbContext.Borrows.Where(x => x.UserId == userId).ToList();

               

                foreach (var item in borrows)
                {
                    DateTime date3 = item.ReturnDate.Date;
                    DateTime date4 = DateTime.Now.Date;

                    TimeSpan timeDifference1 = date3 - date4;
                    int daysDifference1 = timeDifference1.Days;

                    if (item.IsReturned == false && daysDifference1 < 0  )
                    {

                        ViewBag.NotReturn = true;

                        break;
                    }
                    if (borrows.Count >= 2)
                    {
                        ViewBag.bor = true;
                        break;
                    }

                }
            
              
                var book = _dbContext.Books.FirstOrDefault(x => x.Id == id);

                if (book != null)
                {
                    return View(book);
                }

                return View();
            }
            return View();
        }
    }
}
