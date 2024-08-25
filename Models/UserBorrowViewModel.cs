using BookReaders.Areas.AccountUser.Models;

namespace BookReaders.Models
{
    public class UserBorrowViewModel
    {
        
        public int Id { get; set; }
        public ApplicationUser User { get; set; }

        
        public ICollection<Borrow>  Borrows { get; set; }
    }
}
