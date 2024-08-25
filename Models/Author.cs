using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReaders.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Author Name")]
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Auther Biography")]
        [Column(TypeName = "nvarchar(200)")]
        [StringLength(200)]
        public string Biography { get; set; }
        [Required]
        public string Nationality { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Auther Image")]
        public string? ImagePath { get; set; }
        public virtual ICollection<Book> Books { get; set; }



    }
}




