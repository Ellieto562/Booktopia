namespace Booktopia.Models.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int GenreID { get; set; }
        public Genre Genre { get; set; }
        public ICollection<ReadingList> ReadingList { get; set; } = new List<ReadingList>();
    }
}
