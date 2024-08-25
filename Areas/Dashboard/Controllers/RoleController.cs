using BookReaders.Areas.Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;

        }
      

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var roleViewModels = roles.Select(role => new RoleViewModel
            {
                Name = role.Name

            }).ToList();

            return View(roleViewModels);
        }
        [HttpPost]

        public async Task<IActionResult> Create(RoleViewModel role)
        {
          /*  if (!ModelState.IsValid)
            {
                return View("Index", await _roleManager.Roles.ToListAsync());
            }*/
            //check if role name is exist
            var isRoleExists = await _roleManager.RoleExistsAsync(role.Name);
            if (isRoleExists)
            {

                ModelState.AddModelError("Name", "Role is Exist");
                return View("Index", await _roleManager.Roles.ToListAsync());

            }
            await _roleManager.CreateAsync(new IdentityRole { Name = role.Name.Trim() });

            return RedirectToAction(nameof(Index));
        }

    }
}
