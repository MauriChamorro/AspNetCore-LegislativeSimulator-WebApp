namespace WebAppMVC.Models;

public class Person
{
    public int PersonId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required int Age { get; set; }
}