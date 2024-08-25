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
    public class FavoriteController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IRepository<Favorite> _repository;
        public FavoriteController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment, IRepository<Favorite> repository)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _repository = repository;
        }
        public IActionResult Index()
        {
            var bookFav = _repository.GetAll().Include(x => x.Book).Include(y => y.User).ToList();
            return View(bookFav);
        }
    }
}
