namespace BookReaders.Models
{
    public class BookDetailsViewModel
    {
        public Book Book { get; set; }
        public List<Review> Reviews { get; set; }
        public string NewReviewComment { get; set; }
    }
}
