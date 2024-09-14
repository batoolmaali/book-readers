using Azure;
using BookAPI.ViewModels;
using BookReaders.Data;
using BookReaders.Models;
using BookReaders.Repository.Base;
using BookReaders.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class KidsVideosController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;
        Uri baseUri = new Uri("http://localhost:5026/api");

        private readonly IWebHostEnvironment _webHostEnvironment;
        public KidsVideosController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment
           )
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseUri;
        }
        public IActionResult Index()
        {
            HttpResponseMessage response = _httpClient.GetAsync(baseUri + "/KidsVideos").Result;

            if (response.IsSuccessStatusCode)
            {
                var videos = response.Content.ReadAsStringAsync().Result;
                List<KidsVideosViewModel> videosList = JsonConvert.DeserializeObject<List<KidsVideosViewModel>>(videos);
                return View(videosList);
            }
            return View();
        }

        public IActionResult Create()
        {

            return View();
        }

      

        [HttpPost]
        public IActionResult Create(FormVideoKids model)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseUri + "/KidsVideos");

                    // Prepare MultipartFormDataContent to send file along with other form data
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(model.Title), "Title");
                    content.Add(new StringContent(model.Description), "Description");
                    // Add ThumbnailUrl file
                    if (model.ThumbnailUrl != null)
                    {
                        var fileContent = new StreamContent(model.ThumbnailUrl.OpenReadStream());
                        content.Add(fileContent, "ThumbnailUrl", model.ThumbnailUrl.FileName);
                    }

                    // Add VideoUrl file
                    if (model.VideoUrl != null)
                    {
                        var fileContent = new StreamContent(model.VideoUrl.OpenReadStream());
                        content.Add(fileContent, "VideoUrl", model.VideoUrl.FileName);
                    }

                    var response = httpClient.PostAsync("KidsVideos", content).Result;

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
            HttpResponseMessage response = await _httpClient.DeleteAsync(baseUri + $"/KidsVideos/{id}");

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
               
         
    
