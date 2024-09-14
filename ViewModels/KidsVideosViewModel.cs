using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookReaders.ViewModels
{
    public class KidsVideosViewModel
    {
   
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }


        [Url]
        public byte[] ThumbnailUrl { get; set; }

        
        [Url]
        public string VideoUrl { get; set; }


       
    }
}
