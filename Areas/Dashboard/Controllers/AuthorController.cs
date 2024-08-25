using BookReaders.Data;
using BookReaders.Models;
using BookReaders.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _dbContext;
        private IRepository<Author> _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private int pageItem;
        public AuthorController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment,
            IRepository<Author> repository)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _repository = repository;
            pageItem = 5;
        }

        [HttpGet]
        public IActionResult Index(int? id)
        {
           
                var author = _repository.GetAll().Include(B => B.Books).ToList();
                return View(author);
          

        }
        /*      public IActionResult Index(int? id)
              {
                  if (id == 0 || id== null)
                  {
                      var author = _repository.GetAll().Include(B => B.Books).ToList().Take(pageItem);
                      return View(author);
                  }
                  else 
                  {
                      var author = _repository.GetAll().Include(B => B.Books).Where(x=>x.Id > id).ToList().Take(pageItem);
                      return View(author);
                  }



              }*/

        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Create(Author author, IFormFile Image)
        {
            if (Image != null)
            {
                string folder = "Images/AuthorImages";
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }


                author.ImagePath = Path.Combine(folder, fileName);
                _dbContext.Authors.Add(author);
                _dbContext.SaveChanges();
                TempData["successData"] = "Author has been Created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]

        public ActionResult EditAuthor(int id)
        {


            var item = _dbContext.Authors.Where(x => x.Id == id).FirstOrDefault();
            if (item != null)
            {
               
                return View(item);
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditAuthor(int id, Author author, IFormFile Image)
        {
            try
            {
                var item = _dbContext.Authors.FirstOrDefault(x => x.Id == id);

                if (item != null)
                {
                    string folder = "Images/AuthorImages";
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Image.CopyTo(stream);
                    }

                   item.Name=author.Name;
                   item.Birthdate = author.Birthdate;
                    item.Biography = author.Biography;
                    item.Nationality = author.Nationality;
                    item.ImagePath = Path.Combine(folder, fileName);

                    _dbContext.SaveChanges();
                    TempData["successData"] = "Book has been Updated successfully";
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch (Exception ex)
            {
                // Log the exception or return an error view
                return View("Error");
            }
        }

        [HttpPost]

        public ActionResult Delete(int id, Author author)
        {
            _repository.DeleteOne(author);
            TempData["successData"] = "Book has been Deleted successfully";
            return RedirectToAction("Index");
        }
        public ActionResult Search(string SearchItem)
        {

            var authors= _dbContext.Authors.AsQueryable();

            if (SearchItem == null)
            {
                authors = _dbContext.Authors.AsQueryable();
            }

            else
            {
                authors = authors.Include(x => x.Books).Where(
                    x => x.Name.Contains(SearchItem)
                   || x.Nationality.Contains(SearchItem)
                   || x.Biography.Contains(SearchItem)
                   || x.Birthdate.Year.ToString().Contains(SearchItem)).AsQueryable();

                return View("Index", authors);
            }

            return View("Index", authors);
        }
    }
}
