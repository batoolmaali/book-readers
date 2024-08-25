using BookReaders.Areas.AccountUser.Models;

namespace BookReaders.Models
{
    public class UserFavViewModel
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}
