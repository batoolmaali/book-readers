using BookReaders.Data;
using BookReaders.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class Question1Controller : Controller
    {
        private readonly AppDbContext _dbContext;


        public Question1Controller(AppDbContext dbContext
           )
        {
            _dbContext = dbContext;

        }
        public IActionResult Index()
        {
            var Qus = _dbContext.Questions.Include(c => c.User).Include(a => a.Answers).ToList();
            return View(Qus);
        }


        [HttpPost]

        public ActionResult Delete(int id)
        {
            var item = _dbContext.Questions.Where(x => x.Id == id).FirstOrDefault();
            _dbContext.Questions.Remove(item);
            _dbContext.SaveChanges();
            TempData["successData"] = "Book has been Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
