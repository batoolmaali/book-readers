using System.ComponentModel.DataAnnotations;

namespace BookReaders.Areas.AccountUser.ViewModels
{
    public class RegisterViewModel
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]

        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm not match")]

        public string ConfirmPassword { get; set; }
      
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public string Gender { get; set; }
        [Required]
        public string City { get; set; }

        [Display(Name = "Image")]
        public string? ImagePath { get; set; }

    }
}
