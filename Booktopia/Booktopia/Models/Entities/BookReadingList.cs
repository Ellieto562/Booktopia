using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booktopia.Models.Entities
{
    public class BookReadingList
    {
        [Key]
        public int Id { get; set; }

        // 📌 Foreign Keys for Many-to-Many Relationship
        [Required]
        public int BookId { get; set; }

        [Required]
        public int ReadingListId { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }

        [ForeignKey("ReadingListId")]
        public ReadingList ReadingList { get; set; }
    }
}
