using BookReaders.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _dbContext;
      
        public AdminController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
         
        }
        public IActionResult AdminIndex()
        {
            ViewBag.Books=_dbContext.Books.Count();
            ViewBag.Authors =_dbContext.Authors.Count();
            ViewBag.Categories =_dbContext.Categories.Count();
            ViewBag.Users=_dbContext.Users.Count();
            var genderGroups = _dbContext.Users
                         .GroupBy(u => u.Gender)
                          .Select(group => new
     {
         Gender = group.Key,
         Count = group.Count()
     })
     .ToList();


            ViewBag.GenderMaleandFemaile = genderGroups;
            var CityGroups = _dbContext.Users
                               .GroupBy(u => u.City)
                                .Select(group => new
                                {
                                    City = group.Key,
                                    Count = group.Count()
                                }).ToList();

            ViewBag.City = CityGroups;

            var averageAge = _dbContext.Users.Average(u => Convert.ToInt32(DateTime.Now.Year- u.Birthday.Year)); 
            ViewBag.AverageAge = Math.Floor(averageAge);

            //BestBook now represents the book with the highest number of borrows.
            var BestBook = _dbContext.Borrows
                          .GroupBy(u => u.Book.Title)
                           .Select(group => new
                           {
                               book = group.Key,
                               Count = group.Count()
                           }).OrderByDescending(x => x.Count)
                            .FirstOrDefault();

            ViewBag.BestBook = BestBook;

            return View();
        }
    }
}
