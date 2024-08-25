using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BookReaders.Components
{
    public class FavoriteShared : ViewComponent
    {
        private readonly AppDbContext _DbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoriteShared(AppDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _DbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string currentUserId)
        {
           

            if (currentUserId != null)
            {
                var favCount = _DbContext.Favorites.Count(x => x.UserId.ToString() == currentUserId);
                return View("Default", favCount);
            }

            // Handle if current user is null
            return View("Default", 0); // Or handle the case appropriately
        }
    }
}