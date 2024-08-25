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
    public class CategoryController : Controller
    {
        private readonly AppDbContext _dbContext;
      
        private IRepository<Category> _repository;
        public CategoryController(AppDbContext dbContext, IRepository<Category> repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }
        [HttpGet]
        public IActionResult Index()
        {
              var Cat = _repository.GetAll().Include(cat => cat.Books).ToList();
           /* var Cat = _repository.GetAll();*/
        
            return View(Cat);
        }
        [HttpGet]
        public IActionResult Create()
        {
          
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            _repository.Add(category);
			TempData["successData"] = "Category has been Created successfully";
			return RedirectToAction("Index");
        }

        [HttpPost]

        public ActionResult Delete(int id, Category category)
        {
            _repository.DeleteOne(category);
            return RedirectToAction("Index");
        }
       
            public ActionResult Search(string SearchItem)
            {
                var Cat = _dbContext.Categories.AsQueryable();

                if (SearchItem != null)
                {
                    Cat = Cat.Include(x=>x.Books).Where(x => x.Name.Contains(SearchItem)).AsQueryable();
                }

                return View("Index", Cat);
            }
    }
}
