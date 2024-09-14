using BookReaders.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookReaders.Controllers
{
    public class VideoKidController : Controller
    {
        private readonly AppDbContext _dbContext;
   
       
        public VideoKidController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
           

        }
        public IActionResult Index()
        {
            var videos = _dbContext.videoKids.ToList();
            return View(videos);

        }
    }
}
