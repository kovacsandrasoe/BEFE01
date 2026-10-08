using AutoMapper;
using BEFE01.Data;
using BEFE01.Dtos;
using BEFE01.Models;
using BEFE01.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly BookBackgroundService _service;
        public BookController(BookDbContext context, IMapper mapper, UserManager<AppUser> userManager, BookBackgroundService service)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _service = service;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BookViewDto>> GetBook(Guid id)
        {
            var book = await _context.Books.FirstAsync(t => t.Id == id);
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
        [Authorize]
        public async Task<IActionResult> CreateBook(BookCreateDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            var entity = _mapper.Map<Book>(dto);
            if (user != null)
            {
                entity.CreatorId = user.Id;
                _context.Books.Add(entity);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = await _context.Books.FirstAsync(t =>t.Id == id);
            BackgroundJob.Schedule(() => _service.Job(book.AuthorId, _context), TimeSpan.FromSeconds(10));
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok();

            //hangfire lehetőségek
            //aszinkron job: BackgroundService.Enqueue(job)
            //késleltetett job: BackgroundJob.Schedule(job, mikor)
            //ismétlődő job: RecurringJob.AddOrUpdate(job, cron kifejezés)
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
