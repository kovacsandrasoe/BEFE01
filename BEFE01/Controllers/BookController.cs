using BEFE01.Data;
using BEFE01.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<Book> GetBooks()
        {
            
        }
    }
}
