using BookReaders.Models;

namespace BookReaders.ViewModels
{
    public class BookCatVM
    {
        public int Id { get; set; }


        public string Name { get; set; }


        public byte[] Image { get; set; }

        public List<BookKidVM> Books { get; set; }
    }
}
