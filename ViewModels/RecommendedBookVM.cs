using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookReaders.ViewModels
{
    public class RecommendedBookVM
    {
     
        public int Id { get; set; }

     
        public string Title { get; set; }
    
        public string Description { get; set; }

      
        public string Author { get; set; }

        public byte[] ImagePath { get; set; }
        [Display(Name = "Season")]
        public Season? AssociatedSeason { get; set; }
        [Display(Name = "Mood")]
        public Mood? AssociatedMood { get; set; }




    }

    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }

    public enum Mood
    {
        Happy,
        Cozy,
        Adventure,
        Bad
    }
}
