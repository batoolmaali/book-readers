using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookReaders.Components
{
    public class ArabicBooks:ViewComponent
    {
        private readonly AppDbContext _DbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArabicBooks(AppDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _DbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var books=_DbContext.Books.Where(x=>x.Language == "Arabic").Take(6).ToList();
            var ALLBooks = _DbContext.Books.Where(x => x.Language == "Arabic").ToList();
             ViewBag.Books = ALLBooks.Count();
            return View("Default", books);
        }
    }
}
