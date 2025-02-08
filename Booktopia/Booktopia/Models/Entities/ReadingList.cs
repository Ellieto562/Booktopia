namespace Booktopia.Models.Entities
{
    public class ReadingList
    {
        public int ReadingListId { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
