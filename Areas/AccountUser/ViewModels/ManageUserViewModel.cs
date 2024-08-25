namespace BookReaders.Areas.AccountUser.ViewModels
{
    public class ManageUserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public DateTime Birthday { get; set; }

        public string? ImagePath { get; set; }
    }
}

