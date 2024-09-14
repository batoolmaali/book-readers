using BookReaders.Data;
using BookReaders.Models;
using BookReaders.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class ReviewController : Controller
    {

        private readonly AppDbContext _dbContext;
     

        public ReviewController(AppDbContext dbContext
           )
        {
            _dbContext = dbContext;
           
        }
        public IActionResult Index()
        {

            var rev = _dbContext.Reviews.Include(c => c.User).Include(a => a.Book).ToList();
            return View(rev);
           
        }

        [HttpPost]

        public ActionResult Delete(int id)
        {
            var item = _dbContext.Reviews.Where(x => x.Id == id).FirstOrDefault();
            _dbContext.Reviews.Remove(item);
            _dbContext.SaveChanges();
            TempData["successData"] = "Book has been Deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
