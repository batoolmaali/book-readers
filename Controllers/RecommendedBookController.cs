using BookReaders.Data;
using BookReaders.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookReaders.Controllers
{
    public class RecommendedBookController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;


        Uri baseUri = new Uri("http://localhost:5026/api");
        public RecommendedBookController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;

        }
        public async Task<IActionResult> Recommend(string name)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{baseUri}/RecommendedBook/GetBooks/{name}");

            if (response.IsSuccessStatusCode)
            {
                var booksJson = await response.Content.ReadAsStringAsync();
                List<RecommendedBookVM> booksList = JsonConvert.DeserializeObject<List<RecommendedBookVM>>(booksJson);

             


                return View(booksList);
            }
            else
            {
                // Handle unsuccessful response here
                return View();
            }
        }
    }
}
