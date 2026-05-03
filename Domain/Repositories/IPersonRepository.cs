using WebAppMVC.Domain.Models;
using WebAppMVC.Domain.Models.Persons;

namespace WebAppMVC.Domain.Repositories;

public interface IPersonRepository
{
    Task<List<Person>> GetAllPersons();
}