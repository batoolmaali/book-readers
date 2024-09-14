using System.ComponentModel.DataAnnotations;

namespace BookReaders.ViewModels
{
    public class FormVideoKids
    {
        public int Id { get; set; }

     
        public string Title { get; set; }

   
        public string Description { get; set; }


       
        public IFormFile ThumbnailUrl { get; set; }


     
        public IFormFile VideoUrl { get; set; }
    }
}
