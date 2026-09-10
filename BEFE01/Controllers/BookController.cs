using BEFE01.Data;
using BEFE01.Dtos;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return book;
        }

        [HttpGet]
        public async Task<List<BookViewDto>> GetBooks([FromQuery] int? fromYear)
        {
            if (fromYear.HasValue)
            {
                return await _context.Books.Where(b => b.Year >= fromYear.Value).Select(b => new BookViewDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    AuthorName = b.Author != null ? b.Author.Name : string.Empty
                }).ToListAsync();
            }
            return await _context.Books.Select(b => new BookViewDto
            {
                Id = b.Id,
                Title = b.Title,
                Year = b.Year,
                AuthorName = b.Author != null ? b.Author.Name : string.Empty
            }).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookCreateDto dto)
        {
            if (dto.Title.Length < 3)
            {
                return BadRequest("Title must be at least 3 characters long.");
            }
            _context.Books.Add(new Book()
            {
                Title = dto.Title,
                Year = dto.Year
            });
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, Book updatedBook)
        {
            if (id != updatedBook.Id)
            {
                return BadRequest("ID mismatch.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            book.Title = updatedBook.Title;
            book.Year = updatedBook.Year;
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
