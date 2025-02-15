using Booktopia.Data;
using Booktopia.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Booktopia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RatingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ratings = _context.BookRatings.Include(r => r.Book).Include(r => r.User).ToList();
            return View(ratings);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(int bookId, int ratingValue)
        {
            var rating = new BookRating { BookId = bookId, Value = ratingValue };
            _context.BookRatings.Add(rating);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Rate(BookRating rating)
        {
            if (ModelState.IsValid)
            {
                _context.BookRatings.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Book");
            }

            return View(rating);
        }

        [HttpPost]
        public async Task<IActionResult> RateBook(int bookId, int ratingValue)
        {
            var userManager = HttpContext.RequestServices.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Users>>();
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var book = await _context.Books.Include(b => b.Ratings)
                                           .FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            // Check if user already rated this book
            var existingRating = await _context.BookRatings
                .FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == user.Id);

            if (existingRating != null)
            {
                existingRating.Value = ratingValue;
            }
            else
            {
                var rating = new BookRating
                {
                    BookId = bookId,
                    UserId = user.Id,
                    Value = ratingValue
                };
                _context.BookRatings.Add(rating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Book", new { id = bookId });
        }



    }
}

