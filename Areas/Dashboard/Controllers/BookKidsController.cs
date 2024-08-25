using BookAPI.ViewModels;
using BookReaders.Data;
using BookReaders.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class BookKidsController : Controller
    {

        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;
        Uri baseUri = new Uri("http://localhost:5026/api");

        public BookKidsController(AppDbContext dbContext
        )
        {
            _dbContext = dbContext;
         
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;
        }
        public IActionResult Index()
        {
            HttpResponseMessage response = _httpClient.GetAsync(baseUri + "/BookKid").Result;

            if (response.IsSuccessStatusCode)
            {
                var videos = response.Content.ReadAsStringAsync().Result;
                List<BookKidVM> videosList = JsonConvert.DeserializeObject<List<BookKidVM>>(videos);
                return View(videosList);
            }
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            HttpResponseMessage response = _httpClient.GetAsync(baseUri + "/KidsCategory").Result;

            if (response.IsSuccessStatusCode)
            {
                var videos = response.Content.ReadAsStringAsync().Result;
                List<KidsCategoryViewModel> videosList = JsonConvert.DeserializeObject<List<KidsCategoryViewModel>>(videos);
                if(videosList != null)
                {
                    ViewBag.Cat = videosList;
                }
             
            }
               
                return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FormBookKidVM Kidmodel)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseUri + "/BookKid");

                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(Kidmodel.Title), "Title");
                    content.Add(new StringContent(Kidmodel.Description), "Description");
                    content.Add(new StringContent(Kidmodel.KidsCategoryId.ToString()), "KidsCategoryId");
                    content.Add(new StreamContent(Kidmodel.Image.OpenReadStream()), "Image", Kidmodel.Image.FileName);
                    content.Add(new StreamContent(Kidmodel.PDF.OpenReadStream()), "PDF", Kidmodel.PDF.FileName);

                    var response = httpClient.PostAsync("BookKid", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Handle unsuccessful response
                       /* string errorMessage = await response.Content.ReadAsStringAsync();*/
                        // Log or handle the error in your application
                    }
                }
            }

            return View(Kidmodel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(baseUri + $"/BookKid/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Handle successful deletion
                return RedirectToAction("Index"); // Redirect to a different action after successful deletion
            }
            else
            {
                // Handle unsuccessful deletion
                // You might want to display an error message here
                return View(); // Return the view with appropriate error handling
            }
        }
    }
}
