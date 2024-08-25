
using System.ComponentModel.DataAnnotations;

namespace BookReaders.Areas.AccountUser.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }

    
    }
}
