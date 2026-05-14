using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Models.Persons;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Infrastructure.DbContexts;

namespace WebAppMVC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly ExpedientesDevContext _ctx;
    private readonly IPersonRepository _personRepository;

    public PersonController(ExpedientesDevContext ctx, IPersonRepository personRepository)
    {
        _ctx = ctx;
        _personRepository = personRepository;
    }

    // GET: api/Person
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
    {
        //return await _context.Persons.ToListAsync();
        return await _personRepository.GetAllPersons();
    }

    // GET: api/Person/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPerson(int id)
    {
        throw new NotImplementedException();
        /*
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return NotFound();
        return person;
        */
    }

    // PUT: api/Person/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerson(int id, Person person)
    {
        throw new NotImplementedException();

        /*
        if (id != person.PersonId) return BadRequest();

        _context.Entry(person).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PersonExists(id)) return NotFound();

            throw;
        }

        return NoContent();*/
    }

    // POST: api/Person
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Person>> PostPerson(Person person)
    {
        throw new NotImplementedException();

/*
try
{
   _context.Persons.Add(person);
   await _context.SaveChangesAsync();

   return CreatedAtAction("GetPerson", new { id = person.PersonId }, person);
}
catch (Exception e)
{
   return BadRequest(e.Message);
}
 */
    }

// DELETE: api/Person/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        throw new NotImplementedException();

        /*
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return NotFound();

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();

        return NoContent();
        }

        private bool PersonExists(int id)
        {
        return _context.Persons.Any(e => e.PersonId == id);
        }*/
    }
}