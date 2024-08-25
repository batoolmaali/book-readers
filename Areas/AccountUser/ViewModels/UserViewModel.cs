namespace BookReaders.Areas.AccountUser.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string? Gender { get; set; }
        public string? City { get; set; }
        public string? ImagePath { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
