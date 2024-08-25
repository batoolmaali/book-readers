using BookReaders.Areas.AccountUser.Models;
using BookReaders.Areas.AccountUser.ViewModels;
using BookReaders.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol;
using System;
using System.Data;
using System.Security.Claims;

namespace BookReaders.Areas.AccountUser.Controllers
{
    [Area("AccountUser")]
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                if (Image != null)
                {
                    string folder = "Images/UserImages";
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Image.CopyTo(stream);
                    }


                    user.ImagePath = Path.Combine(folder, fileName);
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Gender = model.Gender;
                    user.Birthday = model.Birthday;
                    user.City = model.City;

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        return RedirectToAction("Login", "Account");

                        
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
         /*   var loginVM = new LoginViewModel()
            {
                Schemes = await _signInManager.GetExternalAuthenticationSchemesAsync()
            };*/

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                var currentUser = await _userManager.GetUserAsync(User);
                if (result.Succeeded)
                {
                    var role = await _userManager.GetRolesAsync(currentUser);
                    if (role.Contains("Admin"))
                    {
         
                        return RedirectToAction("AdminIndex", "Admin", new { area = "Dashboard" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                   
                }
                else
                {

                    ModelState.AddModelError("", "Invalid user or password");
                    return View(model);
                }

            }
            return View(model);

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
  
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
              
                return RedirectToAction("Login", "Account");
            }

            var result = await _userManager.ChangePasswordAsync(currentUser, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                // Password change successful
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
             
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var currentUsername = await _userManager.GetUserAsync(User);
            /*  var user = await _userManager.FindByNameAsync(currentUsername.UserName);*/

            if (currentUsername != null)
            {
                var viewModel = new ManageUserViewModel
                {
                   
                    UserName = currentUsername.UserName,
                    PhoneNumber = currentUsername.PhoneNumber,
                    Gender = currentUsername.Gender,
                    Birthday = currentUsername.Birthday,
                    ImagePath = currentUsername.ImagePath,

                };

                return View(viewModel);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ManageUser(ManageUserViewModel model, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser != null)
                {
                    if (Image != null)
                    {
                        string folder = "Images/UserImages";
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }

                        // Update user properties
                        currentUser.ImagePath = Path.Combine(folder, fileName);
                        currentUser.PhoneNumber = model.PhoneNumber;
                        currentUser.Gender = model.Gender;
                        currentUser.Birthday = model.Birthday;

                        // Save changes to the user entity
                        await _userManager.UpdateAsync(currentUser);
                    }

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            return View(model);
        }
     
    }

    }

