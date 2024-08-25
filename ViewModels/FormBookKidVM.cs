namespace BookReaders.ViewModels
{
    public class FormBookKidVM
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public IFormFile Image { get; set; }

        public IFormFile PDF { get; set; }
        public int KidsCategoryId { get; set; }
    }
}
