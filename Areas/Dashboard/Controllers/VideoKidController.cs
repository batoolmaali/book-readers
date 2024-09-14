using BookReaders.Data;
using BookReaders.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class VideoKidController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchItem { get; set; }
        public VideoKidController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            // Initialize 
            PageSize = 10;
            CurrentPage = 1;
            TotalPages = 0;
            SearchItem = "";

        }
        public IActionResult Index(int currentpage = 1)
        {
            int pageSize = 3;
            var videos =  _dbContext.videoKids.ToList();
            var filteredBooks = videos.ToList();

            int totalRecords = filteredBooks.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var paginatedBooks = filteredBooks.Skip((currentpage - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToList();

            ViewBag.CurrentPage = currentpage;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
           

            return View(paginatedBooks);

          
              
        }

        [HttpGet]
        public IActionResult Create()
        {
      
            return View();
        }

        [HttpPost]
        public IActionResult Create(VideoKid video, IFormFile viedoFile,IFormFile Image )
        {
            if (video != null)
            {
                string folder = "Videos/KidsVideos";
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(viedoFile.FileName);
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    viedoFile.CopyTo(stream);
                }


                video.VideoUrl = Path.Combine(folder, fileName);
            }
            if (Image != null)
            {
                string folder1 = "Videos/KidsImages";
                string fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                string filePath1 = Path.Combine(_webHostEnvironment.WebRootPath, folder1, fileName1);

                using (var stream = new FileStream(filePath1, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }


                video.ThumbnailUrl = Path.Combine(folder1, fileName1);

                _dbContext.videoKids.Add(video);
                _dbContext.SaveChanges();
                TempData["successData"] = "Book has been Created successfully";

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]

        public ActionResult Delete(int id)
        {
            var item = _dbContext.videoKids.Where(x => x.Id == id).FirstOrDefault();
            _dbContext.videoKids.Remove(item);
            _dbContext.SaveChanges();
            TempData["successData"] = "Book has been Deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
