using BookAPI.ViewModels;
using BookReaders.Data;
using BookReaders.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class RecommendedBookController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;
        Uri baseUri = new Uri("http://localhost:5026/api");
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchItem { get; set; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        public RecommendedBookController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment
           )
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;
            // Initialize 
            PageSize = 10;
            CurrentPage = 1;
            TotalPages = 0;
            SearchItem = "";
        }



        public IActionResult Index(int currentpage = 1)
        {
            int pageSize = 6;
            HttpResponseMessage response = _httpClient.GetAsync(baseUri + "/RecommendedBook").Result;

            if (response.IsSuccessStatusCode)
            {
                var books = response.Content.ReadAsStringAsync().Result;
                List<RecommendedBookVM> booksList = JsonConvert.DeserializeObject<List<RecommendedBookVM>>(books);

                var filteredBooks = booksList.ToList();

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
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
        
            return View();
        }

        [HttpPost]
        public IActionResult Create(FormRecommendedBook model)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseUri + "/RecommendedBook");

                    // Prepare MultipartFormDataContent to send file along with other form data
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(model.Title), "Title");
                    content.Add(new StringContent(model.Description), "Description");
                    content.Add(new StringContent(model.Author), "Author");
                    // Convert the enum values to strings
                    content.Add(new StringContent(model.AssociatedSeason.ToString()), "AssociatedSeason");
                    content.Add(new StringContent(model.AssociatedMood.ToString()), "AssociatedMood");
                    content.Add(new StreamContent(model.ImagePath.OpenReadStream()), "ImagePath", model.ImagePath.FileName);



                    var response = httpClient.PostAsync("RecommendedBook", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }


            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(baseUri + $"/RecommendedBook/{id}");

            if (response.IsSuccessStatusCode)
            {
              
                return RedirectToAction("Index"); 
            }
            else
            {
               
                return View(); 
            }
        }


     

    }
}
