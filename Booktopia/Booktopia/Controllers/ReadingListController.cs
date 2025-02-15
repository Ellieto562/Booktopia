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
    public class ReadingListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReadingListController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var readingLists = _context.ReadingLists.ToList();
            return View(readingLists);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(int bookId)
        {
            var readingList = new ReadingList { BookId = bookId };
            _context.ReadingLists.Add(readingList);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
