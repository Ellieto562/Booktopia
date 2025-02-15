using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Booktopia.Data;
using Booktopia.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Booktopia.Controllers
{
    [Authorize]
    [Route("ReadingList")]
    public class ReadingListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReadingListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 Get all reading lists for the logged-in user
        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var readingLists = _context.ReadingLists
                                       .Where(rl => rl.UserId == userId)
                                       .Include(rl => rl.BookReadingLists)
                                       .ThenInclude(brl => brl.Book)
                                       .ToList();
            return View(readingLists);
        }

        // 📌 Create a new reading list
        [HttpPost("Create")]
        public IActionResult Create(string name)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Reading list name cannot be empty.");
            }

            var newList = new ReadingList
            {
                Name = name,
                UserId = userId
            };

            _context.ReadingLists.Add(newList);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 📌 Add a book to a reading list
        [HttpPost("AddBook")]
        public IActionResult AddBook(int listId, int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var readingList = _context.ReadingLists.FirstOrDefault(rl => rl.Id == listId && rl.UserId == userId);

            if (readingList == null)
            {
                return NotFound("Reading list not found.");
            }

            var book = _context.Books.Find(bookId);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            // Check if book already exists in the list
            bool bookExists = _context.BookReadingLists.Any(brl => brl.ReadingListId == listId && brl.BookId == bookId);
            if (!bookExists)
            {
                var bookReadingList = new BookReadingList
                {
                    BookId = bookId,
                    ReadingListId = listId
                };
                _context.BookReadingLists.Add(bookReadingList);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // 📌 Remove a book from a reading list
        [HttpPost("RemoveBook")]
        public IActionResult RemoveBook(int listId, int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bookReadingList = _context.BookReadingLists.FirstOrDefault(brl => brl.ReadingListId == listId && brl.BookId == bookId);

            if (bookReadingList == null)
            {
                return NotFound("Book not found in the reading list.");
            }

            _context.BookReadingLists.Remove(bookReadingList);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 📌 Rename a reading list
        [HttpPost("Rename")]
        public IActionResult Rename(int listId, string newName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var readingList = _context.ReadingLists.FirstOrDefault(rl => rl.Id == listId && rl.UserId == userId);

            if (readingList == null)
            {
                return NotFound("Reading list not found.");
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                return BadRequest("New name cannot be empty.");
            }

            readingList.Name = newName;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 📌 Delete a reading list (removes books too)
        [HttpPost("Delete")]
        public IActionResult Delete(int listId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var readingList = _context.ReadingLists
                                      .Include(rl => rl.BookReadingLists)
                                      .FirstOrDefault(rl => rl.Id == listId && rl.UserId == userId);

            if (readingList == null)
            {
                return NotFound("Reading list not found.");
            }

            // Remove books from the list
            _context.BookReadingLists.RemoveRange(readingList.BookReadingLists);

            // Remove the reading list itself
            _context.ReadingLists.Remove(readingList);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
