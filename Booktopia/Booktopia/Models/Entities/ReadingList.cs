namespace Booktopia.Models.Entities
{
    public class ReadingList
    {
        public int ReadingListId { get; set; }
        public string Name { get; set; }
        public int UserID { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<Users> Users { get; set; } = new List<Users>();
    }
}
