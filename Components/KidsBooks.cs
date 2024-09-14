/*using BookReaders.Data;
using BookReaders.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookReaders.Components
{
    public class KidsBooks : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;
        Uri baseUri = new Uri("http://localhost:5026/api");


        public KidsBooks(AppDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;

        }
        [HttpGet]
        public async Task<IViewComponentResult> Invoke()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(baseUri + "/BookKid");

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                List<BookKidVM> bookList = JsonConvert.DeserializeObject<List<BookKidVM>>(res);

                return View("Default", bookList.Take(5));
            }
            else
            {
                return View();
            }
        }

    }
}
*/