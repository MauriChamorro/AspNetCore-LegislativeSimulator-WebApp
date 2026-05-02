using Microsoft.EntityFrameworkCore;
using WebAppMVC.Contexts;
using WebAppMVC.Domain.Models;
using WebAppMVC.Domain.Repositories;

namespace WebAppMVC.Infrastructure.Repositories.DbContexts;

public class PersonDbContext : IPersonRepository
{
    private readonly AppDbContext _dbContext;

    public PersonDbContext(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<List<Person>> GetAllPersons()
    {
        return _dbContext.Persons.ToListAsync();
    }
}