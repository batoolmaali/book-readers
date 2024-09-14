using BookReaders.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookReaders.Components
{
    public class CatShared:ViewComponent
    {
        private readonly AppDbContext _DbContext;
        public CatShared(AppDbContext dbContext)
        {
            this._DbContext = dbContext;

        }
        public IViewComponentResult Invoke()
        {
            var Cat = _DbContext.Categories.Where(c=>c.Name != "Arabic").ToList();
          

            return View("Default", Cat);
        }
    }
}

