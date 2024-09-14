namespace BookReaders.ViewModels
{
    public class FormRecommendedBook
    {
        public int Id { get; set; }


        public string Title { get; set; }

        public string Description { get; set; }


        public string Author { get; set; }

        public IFormFile ImagePath { get; set; }

        public Season1? AssociatedSeason { get; set; }
        public Mood1? AssociatedMood { get; set; }

    }

    public enum Season1
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }

    public enum Mood1
    {
        Happy,
        Cozy,
        Adventure,
        Bad
    }


}
