using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booktopia.Models.Entities
{
    public class ReadingList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }  // Ensure the Name column is present

        [Required]
        public string UserId { get; set; }  // Ensure UserId is present

        // 📌 ✅ Add this property to fix the error!
        public List<BookReadingList> BookReadingLists { get; set; } = new List<BookReadingList>();

        [ForeignKey("UserId")]
        public Users User { get; set; }  // Assuming you're using ASP.NET Identity
    }
}
