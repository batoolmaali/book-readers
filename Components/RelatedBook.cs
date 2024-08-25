using BookReaders.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Components
{
    public class RelatedBook:ViewComponent
    {
        private readonly AppDbContext _DbContext;
        public RelatedBook(AppDbContext dbContext)
        {
            this._DbContext = dbContext;

        }
        public IViewComponentResult Invoke(int id)
        {
            var cat=_DbContext.Categories.Include(cat => cat.Books).Where(c => c.Id == id).FirstOrDefault();

            if (cat != null)
            {
                var Cat = cat.Books.Take(5).ToList();
                return View("Default",Cat);
            }
        
           

            return View("Default");
        }
    }
}
