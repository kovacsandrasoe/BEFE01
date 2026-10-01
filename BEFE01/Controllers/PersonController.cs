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
    public class PersonController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IMapper _mapper;
        public PersonController(BookDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDetailedViewDto>> GetPerson(Guid id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return _mapper.Map<PersonDetailedViewDto>(person);
        }

        [HttpGet]
        public async Task<List<PersonShortViewDto>> GetPeople([FromQuery] int? minAge)
        {
            if (minAge.HasValue)
            {
                return await _context.People.Where(p => p.Age >= minAge.Value).Select(z => _mapper.Map<PersonShortViewDto>(z)).ToListAsync();
            }
            return await _context.People.Select(z => _mapper.Map<PersonShortViewDto>(z)).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(PersonCreateDto dto)
        {
            if (dto.Name.Length < 2)
            {
                return BadRequest("Name must be at least 2 characters long.");
            }
            if (dto.Age < 0)
            {
                return BadRequest("Age must be non-negative.");
            }
            _context.People.Add(_mapper.Map<Person>(dto));
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
        public async Task<IActionResult> UpdatePerson(Guid id, PersonUpdateDto dto)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            _mapper.Map(dto, person);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
