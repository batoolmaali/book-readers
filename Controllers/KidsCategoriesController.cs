using Azure;
using BookAPI.ViewModels;
using BookReaders.Data;
using BookReaders.Models;
using BookReaders.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookReaders.Controllers
{
    public class KidsCategoriesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;


        Uri baseUri = new Uri("http://localhost:5026/api");
        public KidsCategoriesController(AppDbContext dbContext)
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
                var res = response.Content.ReadAsStringAsync().Result;
                List<KidsCategoryViewModel> catList = JsonConvert.DeserializeObject<List<KidsCategoryViewModel>>(res);
                return View(catList);
            }
            return View();
        }

        [HttpGet]
        public IActionResult allbook(int id)
        {
            HttpResponseMessage response = _httpClient.GetAsync($"{baseUri}/KidsCategory/GetCategoryWithBooks/{id}").Result;



            if (response.IsSuccessStatusCode)
            {
                var booksJson = response.Content.ReadAsStringAsync().Result;


                List<BookKidVM> booksList = JsonConvert.DeserializeObject<List<BookKidVM>>(booksJson);

                return View(booksList);


            }
            return View();
        }



        /*     [HttpGet]
             public async Task<IActionResult> allbook(int id)
             {
                 HttpResponseMessage response = await _httpClient.GetAsync(baseUri + "/BookKid");

                 if (response.IsSuccessStatusCode)
                 {
                     var res = await response.Content.ReadAsStringAsync();
                     List<BookKidVM> bookList = JsonConvert.DeserializeObject<List<BookKidVM>>(res);

                     // Filter 
                     var filteredbooks = bookList.Where(item => item.KidsCategoryId == id).ToList();

                     return View(filteredbooks);
                 }


                 return View("Error");
             }
     */





    }

}
