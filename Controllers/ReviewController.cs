using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using BookReaders.Models;
using BookReaders.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookReaders.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(AppDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var reviews = _dbContext.Reviews.Include(b => b.Book).Include(u => u.User).OrderByDescending(x => x.Id).ToList();
            return View(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(BookDetailsViewModel viewModel)
        {
            if (ModelState.IsValid)
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

                        // Redirect to BookDetails with the BookId parameter
                        return RedirectToAction("BookDetails", "Book", new { area = "", id = viewModel.Book.Id });
                    }
                    else
                    {
                        // Handle the case where the current username is not found
                        return RedirectToAction("Index", "Home"); // Redirect to the home page or another appropriate action
                    }
                }
                else
                {
                    // Handle the case where the user is not authenticated
                    // You may want to redirect to the login page or display an error message
                    return RedirectToAction("Login", "Account");
                }
            }

            // If model state is invalid, return the same view with the model
            // You should return the same view to display validation errors to the user
            return View("BookDetails", viewModel);
        }

        [HttpPost]

        public ActionResult Delete(int id)
        {
            var item = _dbContext.Reviews.Where(x => x.Id == id).FirstOrDefault();

            if (item != null)
            {
                _dbContext.Reviews.Remove(item);

                _dbContext.SaveChanges();

                int bookId =item.BookId;
                return RedirectToAction("BookDetails", "Book", new { id = bookId, area = "" });
            }
            return View();
        }
    }
}