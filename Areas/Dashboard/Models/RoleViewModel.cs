using System.ComponentModel.DataAnnotations;

namespace BookReaders.Areas.Dashboard.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}
