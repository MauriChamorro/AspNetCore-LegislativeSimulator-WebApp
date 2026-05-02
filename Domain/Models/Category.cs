using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Domain.Models;

public class Category
{
    public int? Id { get; set; }

    [Required] [DisplayName("Nombre")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}