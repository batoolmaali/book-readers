using BookReaders.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Components
{
    public class ComingSoon:ViewComponent
    {
        private readonly AppDbContext _DbContext;
        public ComingSoon(AppDbContext dbContext)
        {
            this._DbContext = dbContext;

        }
        public IViewComponentResult Invoke()
        {
            var books = _DbContext.Books.AsQueryable();
            var come= books.Where(x=>x.IsComingSoon).Take(5).ToList();

            return View("Default", come);
        }
    }
}
