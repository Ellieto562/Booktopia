using Booktopia.Data;
using Booktopia.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booktopia.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            var books = _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Include(b => b.Ratings)
            .ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString)).ToList();
            }

            return View(books);
        }


        [Authorize]
        public IActionResult Add() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Add(Book book)
        {
            
            ViewBag.Authors = new SelectList(_context.Authors, "Id", "Name");
            ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name");
            if (ModelState.IsValid)
            {

                _context.Books.Add(book);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        [Authorize]
        public IActionResult Update(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Update(book);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        [Authorize]
        public IActionResult Remove(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        [Authorize]
        public IActionResult RemoveConfirmed(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound();

            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

