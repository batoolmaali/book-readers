using BookAPI.ViewModels;
using BookReaders.Data;
using BookReaders.Models;
using BookReaders.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class KidsCategory1Controller : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;


        Uri baseUri = new Uri("http://localhost:5026/api");
        public KidsCategory1Controller(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;

        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpResponseMessage response = _httpClient.GetAsync(baseUri + "/KidsCategory").Result;

            if (response.IsSuccessStatusCode)
            {
                var videos = response.Content.ReadAsStringAsync().Result;
                List<KidsCategoryViewModel> videosList = JsonConvert.DeserializeObject<List<KidsCategoryViewModel>>(videos);
                return View(videosList);
            }
            return View();
        }
     


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(FormKidsCategoryVM model)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseUri + "/KidsCategory");

                    // Prepare MultipartFormDataContent to send file along with other form data
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(model.Name), "Name");
                    content.Add(new StreamContent(model.Image.OpenReadStream()), "Image", model.Image.FileName);

                    var response = httpClient.PostAsync("KidsCategory", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            // If the model state is not valid or the request is not successful, return the view with the model
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(baseUri + $"/KidsCategory/{id}");

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

