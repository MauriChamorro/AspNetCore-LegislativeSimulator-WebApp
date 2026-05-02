using WebAppMVC.Domain.Models;

namespace WebAppMVC.Domain.Repositories;

public interface IPersonRepository
{
    Task<List<Person>> GetAllPersons();
}