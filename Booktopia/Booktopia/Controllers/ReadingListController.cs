using Booktopia.Data;
using Booktopia.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Booktopia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Only authenticated users can access
    public class ReadingListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReadingListController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  GET: api/readinglist - Fetch user’s reading list
        [HttpGet]
        public async Task<IActionResult> GetUserReadingList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var readingList = await _context.ReadingLists
                .Where(rl => rl.UserId == userId)
                .Include(rl => rl.Book)
                .ToListAsync();

            return Ok(readingList.Select(rl => rl.Book));
        }

        //  POST: api/readinglist/{bookId} - Add book to reading list
        [HttpPost("{bookId}")]
        public async Task<IActionResult> AddToReadingList(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return NotFound("Book not found");

            var existingEntry = await _context.ReadingLists
                .FirstOrDefaultAsync(rl => rl.UserId == userId && rl.BookId == bookId);

            if (existingEntry != null)
                return BadRequest("Book already in reading list");

            var readingListEntry = new ReadingList { UserId = userId, BookId = bookId };
            _context.ReadingLists.Add(readingListEntry);
            await _context.SaveChangesAsync();

            return Ok("Book added to reading list");
        }

        // DELETE: api/readinglist/{bookId} - Remove book from reading list
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> RemoveFromReadingList(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var readingListEntry = await _context.ReadingLists
                .FirstOrDefaultAsync(rl => rl.UserId == userId && rl.BookId == bookId);

            if (readingListEntry == null)
                return NotFound("Book not in reading list");

            _context.ReadingLists.Remove(readingListEntry);
            await _context.SaveChangesAsync();

            return Ok("Book removed from reading list");
        }
    }
}
