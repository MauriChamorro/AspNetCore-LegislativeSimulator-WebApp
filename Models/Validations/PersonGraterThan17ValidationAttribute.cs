using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models.Validations;

public class PersonGraterThan17ValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var aPerson = (Person)validationContext.ObjectInstance;

        if (aPerson != null)
        {
            if (aPerson.Age < 18)
            {
                return new ValidationResult("Age must be equal or greater than 18");
            }
        }
        return ValidationResult.Success;
    }
}