using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using BookReaders.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookReaders.Controllers
{
    public class KidsVideosController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;


        Uri baseUri = new Uri("http://localhost:5026/api");
        public KidsVideosController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;
           

        }

        public  IActionResult Index()
        {
            HttpResponseMessage response =  _httpClient.GetAsync(baseUri+ "/KidsVideos").Result;

            if (response.IsSuccessStatusCode)
            {
                var videos = response.Content.ReadAsStringAsync().Result;
                List<KidsVideosViewModel> videosList = JsonConvert.DeserializeObject<List<KidsVideosViewModel>>(videos);
                return View(videosList);
            }
            return View();
        }
    }
}
