using AutoMapper;
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
        private readonly IMapper _mapper;
        public BookController(BookDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookViewDto>> GetBook(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return _mapper.Map<BookViewDto>(book);
        }

        [HttpGet]
        public async Task<List<BookViewDto>> GetBooks([FromQuery] int? fromYear)
        {
            if (fromYear.HasValue)
            {
                return await _context.Books.Where(b => b.Year >= fromYear.Value)
                    .Select(b => _mapper.Map<BookViewDto>(b)).ToListAsync();
            }
            return await 
                _context.Books.Select(b => _mapper.Map<BookViewDto>(b)).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookCreateDto dto)
        {
            if (dto.Title.Length < 3)
            {
                return BadRequest("Title must be at least 3 characters long.");
            }
            var entity = _mapper.Map<Book>(dto);
            _context.Books.Add(entity);
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
        public async Task<IActionResult> UpdateBook(Guid id, BookUpdateDto dto)
        {
            var book = await _context.Books.FirstAsync(t => t.Id == id);
            _mapper.Map(dto, book);
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
