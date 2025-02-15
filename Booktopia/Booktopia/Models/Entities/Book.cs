using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booktopia.Models.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        [Required]
        public int GenreID { get; set; }
        public Genre Genre { get; set; }

        public ICollection<BookRating> Ratings { get; set; } = new List<BookRating>();

        // ⭐ Compute Average Rating (Read-Only)
        public double GetAverageRating()
        {
            return Ratings.Any() ? Ratings.Average(r => r.Value) : 0;
        }
        [NotMapped]  // Prevents EF from adding this to the database
        public int? NewRating { get; set; }
        public ICollection<ReadingList> ReadingList { get; set; } = new List<ReadingList>();
    }
}
