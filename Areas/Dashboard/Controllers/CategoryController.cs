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
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchItem { get; set; }
        public CategoryController(AppDbContext dbContext, IRepository<Category> repository)
        {
            _dbContext = dbContext;
            _repository = repository;
            // Initialize 
            PageSize = 10;
            CurrentPage = 1;
            TotalPages = 0;
            SearchItem = "";
        }

      
        [HttpGet]

        public IActionResult Index(string SearchItem, int currentpage = 1)
        {


            int pageSize = 4;
            var Cat = _repository.GetAll().Include(cat => cat.Books).AsQueryable();


            if (SearchItem == null)
            {
                Cat = _repository.GetAll().Include(cat => cat.Books).AsQueryable();
            }

            else
            {
              
                    Cat = Cat.Include(x => x.Books).Where(x => x.Name.Contains(SearchItem)).AsQueryable();
             





            }
            // Materialize the query before pagination
            var filteredBooks = Cat.ToList();

            int totalRecords = filteredBooks.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var paginatedBooks = filteredBooks.Skip((currentpage - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToList();

            ViewBag.CurrentPage = currentpage;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchItem = SearchItem;





            return View(paginatedBooks);


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
       
        
    }
}
