using BEFE01.Data;
using BEFE01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BEFE01.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookDbContext _context;
        public BookController(BookDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Book>> GetBooks([FromQuery] int? fromYear)
        {
            if (fromYear.HasValue)
            {
                return await _context.Books.Where(b => b.Year >= fromYear.Value).ToListAsync();
            }
            return await _context.Books.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book)
        {
            if (book.Title.Length < 3)
            {
                return BadRequest("Title must be at least 3 characters long.");
            }
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
