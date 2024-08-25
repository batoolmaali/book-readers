using BookAPI.ViewModels;
using BookReaders.Data;
using BookReaders.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookReaders.Controllers
{
    public class BookKidsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;
        Uri baseUri = new Uri("http://localhost:5026/api");


        public BookKidsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;

        }
        public IActionResult Index()
        {
            return View();
        }

        /*  [HttpGet]
          public async Task<IActionResult> Details(int id)
          {
              HttpResponseMessage response = await _httpClient.GetAsync(baseUri + $"/BookKid/GetById/{id}");

              if (response.IsSuccessStatusCode)
              {
                  var content = await response.Content.ReadAsStringAsync();
                  var book = JsonConvert.DeserializeObject<BookKidVM>(content);

                  return View(book);
              }
              else
              {
                
              
                  var errorContent = await response.Content.ReadAsStringAsync();
                  return Content($"Failed to retrieve book details. StatusCode: {response.StatusCode}. Error: {errorContent}");
              }*/
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUri + $"/BookKid");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var book = JsonConvert.DeserializeObject<List<BookKidVM>>(content);

                // Filter 
                var filteredBook = book.Where(item => item.Id == id).FirstOrDefault();

             

                return View(filteredBook);
            }
            else
            {
               
                var errorContent = await response.Content.ReadAsStringAsync();
                return Content($"Failed to retrieve book details. StatusCode: {response.StatusCode}. Error: {errorContent}");
            }

        }
    }
}