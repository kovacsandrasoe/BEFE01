using BEFE01.Models;
using BEFE01.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BEFE01.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly BookDbContext _context;
        public PersonController(BookDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(Guid id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return person;
        }

        [HttpGet]
        public async Task<List<Person>> GetPeople([FromQuery] int? minAge)
        {
            if (minAge.HasValue)
            {
                return await _context.People.Where(p => p.Age >= minAge.Value).ToListAsync();
            }
            return await _context.People.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person person)
        {
            if (person.Name.Length < 2)
            {
                return BadRequest("Name must be at least 2 characters long.");
            }
            if (person.Age < 0)
            {
                return BadRequest("Age must be non-negative.");
            }
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(Guid id, Person updatedPerson)
        {
            if (id != updatedPerson.Id)
            {
                return BadRequest("ID mismatch.");
            }
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            person.Name = updatedPerson.Name;
            person.Age = updatedPerson.Age;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
