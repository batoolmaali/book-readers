using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BookReaders.Components
{
    public class MessageShared:ViewComponent
    {
        private readonly AppDbContext _DbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageShared(AppDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _DbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string currentUserId)
        {


            if (currentUserId != null)
            {
                var MessageCount = _DbContext.Messages.Count(x => x.UserId.ToString() == currentUserId);
                return View("Default", MessageCount);
            }

            // Handle if current user is null
            return View("Default", 0); // Or handle the case appropriately
        }
    }
}
