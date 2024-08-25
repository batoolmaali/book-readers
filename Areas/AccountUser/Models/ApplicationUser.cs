using BookReaders.Models;
using Microsoft.AspNetCore.Identity;

namespace BookReaders.Areas.AccountUser.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime Birthday { get; set; }
        public string? Gender { get; set; }
        public string? City { get; set; }
        public string? ImagePath { get; set; }

        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Borrow> Borrows { get; set; }

        public List<Review> Reviews { get; set; }

        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }

        public Message Message { get; set; }
    }
}
