namespace Booktopia.Models.Entities
{
    public class ReadingList
    {
        public int ReadingListId { get; set; }
        public string Name { get; set; }
        public string UserID { get; set; }
        public Users User { get; set; } 
        public int BookId { get; set; }
        public Book Book { get; set; }
        
    }
}
