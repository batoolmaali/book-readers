using BookReaders.Areas.AccountUser.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReaders.Models
{
  
        public class Favorite
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
            public string UserId { get; set; }
            public ApplicationUser User { get; set; }
            public int BookId { get; set; }
            public Book Book { get; set; }
        }
    }

