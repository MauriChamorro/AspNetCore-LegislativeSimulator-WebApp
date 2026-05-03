using WebAppMVC.Domain.Models.Persons.Validations;

namespace WebAppMVC.Domain.Models.Persons;

public class Person
{
    public int PersonId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    
    [PersonGraterThan17Validation]
    public required int Age { get; set; }
}