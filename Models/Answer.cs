using BookReaders.Areas.AccountUser.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookReaders.Models
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
