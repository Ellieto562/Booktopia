using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booktopia.Models.Entities
{
    public class BookRating
    {
            public int Id { get; set; }
            public int BookId { get; set; }
            public Book Book { get; set; }

            [Range(1, 5)]
            public int Value { get; set; }

            public string UserId { get; set; } 
            public Users User { get; set; }     

    }
}
