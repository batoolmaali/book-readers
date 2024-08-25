using BookReaders.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Components
{
    public class LatestBooks:ViewComponent
    {
        private readonly AppDbContext _DbContext;
        public LatestBooks(AppDbContext dbContext)
        {
           _DbContext = dbContext;

        }
        public IViewComponentResult Invoke()
        {
          
             var Books = _DbContext.Books.OrderByDescending(b=>b.Id).Take(5).ToList();

            return View("Default", Books);
        }
    }
}
