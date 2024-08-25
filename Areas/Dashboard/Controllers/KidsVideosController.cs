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
            HttpResponseMessage response = _httpClient.GetAsync(baseUri + "/KidsCategory").Result;

            if (response.IsSuccessStatusCode)
            {
                var videos = response.Content.ReadAsStringAsync().Result;
                List<KidsCategoryViewModel> videosList = JsonConvert.DeserializeObject<List<KidsCategoryViewModel>>(videos);
                return View(videosList);
            }
            return View();
        }

        public IActionResult Create()
        {
            var model = new KidsVideosViewModel();
            return View(model);
        }

         [HttpPost]
        public async Task<IActionResult> Create([Bind("Title, Description, ThumbnailUrl, VideoUrl")] KidsVideosViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Process the form data, save to database, etc.
                // You can access form fields using the model parameter

                // Redirect to a success page or return a success response
                return RedirectToAction("Index", "Home");
            }

            // If the model state is not valid, return to the form with validation errors
            return View(model);
        }
    }
        /*   [HttpPost]
           public async Task<IActionResult> Create(KidsVideos kids, IFormFile ThumbnailUrl, IFormFile VideoUrl)
           {


               using (var httpClient = new HttpClient())
               {
                   httpClient.BaseAddress = new Uri(baseUri + "/KidsVideos");
                   var responseMessage = httpClient.PostAsJsonAsync<KidsVideos>("KidsVideos", kids); responseMessage.Wait();
                   var response = responseMessage.Result;
                   if (response.IsSuccessStatusCode)
                   {
                       return RedirectToAction("Index");
                   }
                   return View();
               }
           }*/


  
    
    }
               
         
    
