using BookReaders.Areas.AccountUser.Models;
using BookReaders.Areas.AccountUser.ViewModels;
using BookReaders.Areas.Dashboard.Models;
using BookReaders.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class UserController : Controller
    {
       
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchItem { get; set; }
        public UserController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager
            )
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
            // Initialize 
            PageSize = 10;
            CurrentPage = 1;
            TotalPages = 0;
            SearchItem = "";
        }

       

            public async Task<IActionResult> Index(int currentpage = 1)
            {
            int pageSize = 5;
            var Allusers = await _userManager.Users.ToListAsync();
                var users = new List<UserViewModel>();

                foreach (var user in Allusers)
                {
                    users.Add(new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Gender = user.Gender,
                        City = user.City,
                        PhoneNumber=user.PhoneNumber,
                        Birthday = user.Birthday,
                        ImagePath = user.ImagePath,
                        Roles = _userManager.GetRolesAsync(user).Result
                    });
                }
            var filteredBooks = users.ToList();

            int totalRecords = filteredBooks.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var paginatedBooks = filteredBooks.Skip((currentpage - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToList();

            ViewBag.CurrentPage = currentpage;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;


            return View(paginatedBooks);

           
            }
        public async Task<IActionResult> Manage(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
                Roles = roles.Select(x => new SelectRoleViewModel
                {
                    Id = x.Id,
                    RoleName = x.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, x.Name).Result,
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var rolesToRemove = await _userManager.GetRolesAsync(user);
            var resonse = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (resonse.Succeeded)
            {
                var rolesToAdd = model.Roles.Where(x => x.IsSelected).Select(x => x.RoleName).ToList();
                var res = await _userManager.AddToRolesAsync(user, rolesToAdd);

            }
            return RedirectToAction(nameof(Index));
        }

    }
    }

