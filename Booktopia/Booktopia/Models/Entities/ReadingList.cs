namespace Booktopia.Models.Entities
{
    public class ReadingList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public Users User { get; set; } 
        public int BookId { get; set; }
        public Book Book { get; set; }
        
    }
}
