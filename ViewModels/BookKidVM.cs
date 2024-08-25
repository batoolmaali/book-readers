namespace BookReaders.ViewModels
{
    public class BookKidVM
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public byte[] Image { get; set; }
        public byte[] PDF { get; set; }
        public int KidsCategoryId { get; set; }

       
    }
}
