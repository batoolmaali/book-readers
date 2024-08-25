using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReaders.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Book Name")]
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Book Description")]
        [Column(TypeName = "nvarchar(300)")]
        [StringLength(300)]
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string Language { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Book Image")]
        public string? ImagePath { get; set; }

        [Display(Name = "Book PDF")]
        public string? BookPDF { get; set; }
        [Required]
        public bool IsAvailable { get; set; }

        public bool IsComingSoon { get; set; }

        [Display(Name = "Category Name")]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Display(Name = "Author Name")]
        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public ICollection<Favorite> Favorites { get; set; }

        public List<Review> Reviews { get; set; }

        public ICollection<Borrow> Borrows { get; set; }

    }
}
