using WebAppMVC.Domain.Models.Validations;

namespace WebAppMVC.Domain.Models;

public class Person
{
    public int PersonId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    
    [PersonGraterThan17Validation]
    public required int Age { get; set; }
}