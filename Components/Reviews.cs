using BookReaders.Data;
using BookReaders.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Components
{
    public class Reviews: ViewComponent
    {
        private readonly AppDbContext _DbContext;
        public Reviews(AppDbContext dbContext)
        {
            this._DbContext = dbContext;

        }
        public IViewComponentResult Invoke()
        {
       
            var reviews=_DbContext.Reviews.Include(b=>b.Book).Include(u=>u.User).OrderByDescending(x=>x.Id).Take(3).ToList();

            return View("Default", reviews);
        }
    }
}
