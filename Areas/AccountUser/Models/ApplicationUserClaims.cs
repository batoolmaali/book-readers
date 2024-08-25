using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Principal;

namespace BookReaders.Areas.AccountUser.Models
{
    public class ApplicationUserClaims : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaims(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var Identity= await base.GenerateClaimsAsync(user);
            Identity.AddClaim(new Claim("Id", user.Id ?? ""));
            Identity.AddClaim(new Claim("Email", user.Email ?? ""));
            Identity.AddClaim(new Claim("UserImage", user.ImagePath ?? ""));
            Identity.AddClaim(new Claim("City", user.City ?? ""));
            Identity.AddClaim(new Claim("Gender", user.Gender ?? ""));
            // Check if user.Birthday is not null before converting it to a string
            if (user.Birthday != null)
            {
                Identity.AddClaim(new Claim("Birthday", user.Birthday.ToString()));
            }
            return Identity;
        }


    }
}
