namespace BookReaders.Areas.Dashboard.Models
{
    public class UserRolesViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }

        public List<SelectRoleViewModel> Roles { get; set; }
    }
}
