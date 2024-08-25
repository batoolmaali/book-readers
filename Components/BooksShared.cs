using BookReaders.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Components
{
    public class BooksShared:ViewComponent
    {
        private readonly AppDbContext _DbContext;
        public BooksShared(AppDbContext dbContext)
        {
            this._DbContext = dbContext;

        }
        public IViewComponentResult Invoke()
        {
            var Cat = _DbContext.Categories.Include(cat => cat.Books).ThenInclude(a=>a.Author).ToList();
           /* var Books = _DbContext.Books.Take(6).ToList();*/

            return View("Default", Cat);
        }
    }
}
