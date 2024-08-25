﻿using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using BookReaders.Models;
using BookReaders.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReaders.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly AppDbContext _dbContext;

      
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public QuestionController(AppDbContext dbContext,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }
        public IActionResult Index()
        {
            var Qus=_dbContext.Questions.Include(x=>x.User).Include(a=>a.Answers).OrderByDescending(x=>x.Id).ToList();
            return View(Qus);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Question question)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                question.CreatedAt = DateTime.Now;
                question.UserId = currentUser.Id;

                _dbContext.Questions.Add(question);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "AccountUser" });
            }
            return View();
        }


        [HttpPost]

        public ActionResult Delete(int id, Question question)
        {
            var item = _dbContext.Questions.Where(x => x.Id == id).FirstOrDefault();

            if(item != null)
            {
                _dbContext.Questions.Remove(item);

                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
