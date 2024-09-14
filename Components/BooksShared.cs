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
            var Cat = _DbContext.Categories.Where(c=>c.Name != "Arabic").Include(cat => cat.Books).ThenInclude(a=>a.Author).ToList();
            ViewBag.Books = Cat.Count();
         

            return View("Default", Cat);
        }
    }
}
