using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required] [Range(0, int.MaxValue)]
    public decimal? Price { get; set; }
    [Required]
    public int? Quantity { get; set; }
    [Required]
    public int CategoryId { get; set; }
}