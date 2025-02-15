using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booktopia.Models.Entities
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }  // Change from AuthorId to a string

        [Required(ErrorMessage = "Genre is required.")]
        public string Genre { get; set; }

        public List<BookReadingList> BookReadingLists { get; set; } = new List<BookReadingList>();
    }
}
